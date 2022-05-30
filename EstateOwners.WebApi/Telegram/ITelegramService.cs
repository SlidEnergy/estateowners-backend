using System.Threading.Tasks;

namespace EstateOwners.WebApi
{
	public interface ITelegramService
	{
		Task ConnectTelegramUser(string userId, TelegramUser telegramUser);
	}
}