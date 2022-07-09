using EstateOwners.App;
using EstateOwners.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EstateOwners.WebApi.Dto;
using Microsoft.IdentityModel.Tokens;

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

            var tokens = await _tokenService.GenerateAccessAndRefreshTokens(user, AccessMode.All);

            return new TokenInfo() { Token = tokens.Token, RefreshToken = tokens.RefreshToken, Email = user.Email };
        }

        private bool ValidateTelegramInput(TelegramUser telegramUser)
        {
            var dataCheckString = string.Format("auth_date={0}\nfirst_name={1}\nid={2}\nlast_name={3}\nusername={4}",
                telegramUser.Auth_date, telegramUser.First_name, telegramUser.Id, telegramUser.Last_name, telegramUser.Username);

            using (var sha256 = SHA256.Create())
            {
                var secretKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(_telegramSettings.ApiToken));

                using (var hmac = new HMACSHA256(secretKey))
                {
                    byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString));

                    if (BitConverter.ToString(hashValue).Replace("-", "").ToLower() == telegramUser.Hash)
                        return true;
                }
            }

            return false;
        }
    }
}
