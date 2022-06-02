using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    internal abstract class DialogBase : IUpdateHandler
    {
        public int step { get; set; } = 1;
        public bool activate { get; set; } = false;

        public virtual bool CanHandle(IUpdateContext context) => true;
        public abstract Task HandleAsync(IUpdateContext context, UpdateDelegate next);
    }
}
