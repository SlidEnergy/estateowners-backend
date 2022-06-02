using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    public interface IMenuRenderer
    {
        Task ClearMenu(IUpdateContext context);
        Task RenderMenu(IUpdateContext context);
    }
}