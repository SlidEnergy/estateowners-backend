using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    public interface IMenuRenderer
    {
        Task ClearMenuAsync(IUpdateContext context, CancellationToken cancellationToken = default);
        Task RenderMenuAsync(IUpdateContext context, CancellationToken cancellationToken = default);

        Task RenderInlineMenuAsync(IUpdateContext context, bool renderInlineMenu = false, CancellationToken cancellationToken = default);

        Task SetCommands(IUpdateContext context, CancellationToken cancellationToken = default);

        Task ClearCommandsAsync(IUpdateContext context, CancellationToken cancellationToken = default);
    }
}