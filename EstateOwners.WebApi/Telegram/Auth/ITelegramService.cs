using System.Threading.Tasks;

namespace EstateOwners.WebApi.Telegram.Connect
{
    public interface ITelegramService
    {
        Task ConnectTelegramUser(string userId, TelegramUser telegramUser);
    }
}