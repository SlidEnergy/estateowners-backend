using System.Threading.Tasks;
using EstateOwners.WebApi.Dto;

namespace EstateOwners.WebApi.Telegram.Connect
{
    public interface ITelegramService
    {
        Task ConnectTelegramUser(string userId, TelegramUser telegramUser);

        Task<TokenInfo> GetTokenAsync(TelegramUser telegramUser);
    }
}