using System;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EstateOwners.App;
using EstateOwners.Domain;
using EstateOwners.WebApi.Dto;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace EstateOwners.WebApi.Telegram.Connect
{
    public class TelegramService : ITelegramService
    {
        private IAuthTokenService _authTokenService;
        private readonly IUsersService _usersService;
        private readonly ITokenService _tokenService;
        private TelegramBotSettings _telegramSettings;

        public TelegramService(IAuthTokenService authTokenService, IOptions<TelegramBotSettings> options, IUsersService usersService, ITokenService tokenService)
        {
            _authTokenService = authTokenService;
            _usersService = usersService;
            _tokenService = tokenService;
            _telegramSettings = options.Value;
        }

        public async Task ConnectTelegramUser(string userId, TelegramUser telegramUser)
        {
            if (!ValidateTelegramInput(telegramUser))
            {
                throw new ArgumentException("Данные телеграм пользователя не прошли проверку.", nameof(telegramUser));
            }

            await _authTokenService.AddToken(userId, telegramUser.Id.ToString(), AuthTokenType.TelegramUserId);
        }

        public async Task<TokenInfo> GetTokenAsync(TelegramUser telegramUser)
        {
            if (!ValidateTelegramInput(telegramUser))
            {
                throw new ArgumentException("Данные телеграм пользователя не прошли проверку.", nameof(telegramUser));
            }

            var user = await _usersService.GetByAuthTokenAsync(telegramUser.Id.ToString(), AuthTokenType.TelegramUserId);

            if (user == null)
                throw new AuthenticationException();

            var tokens = await _tokenService.GenerateAccessAndRefreshTokens(user, AccessMode.All);

            return new TokenInfo() { Token = tokens.Token, RefreshToken = tokens.RefreshToken, Email = user.Email };
        }

        private bool ValidateTelegramInput(TelegramUser telegramUser)
        {
            var dataCheckString = new StringBuilder();

            dataCheckString.AppendFormat("auth_date={0}\n", telegramUser.Auth_date);

            if (telegramUser.First_name != null)
                dataCheckString.AppendFormat("first_name={0}\n", telegramUser.First_name);

            dataCheckString.AppendFormat("id={0}\n", telegramUser.Id);

            if (telegramUser.Last_name != null)
                dataCheckString.AppendFormat("last_name={0}\n", telegramUser.Last_name);

            if (telegramUser.Username != null)
                dataCheckString.AppendFormat("username={0}", telegramUser.Username);

            using (var sha256 = SHA256.Create())
            {
                var secretKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(_telegramSettings.ApiToken));

                using (var hmac = new HMACSHA256(secretKey))
                {
                    byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString.ToString()));

                    if (BitConverter.ToString(hashValue).Replace("-", "").ToLower() == telegramUser.Hash)
                        return true;
                }
            }

            return false;
        }
    }
}
