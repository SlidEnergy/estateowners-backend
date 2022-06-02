using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    public class CallbackQueryHandler : UpdateHandlerBase
    {
        public override bool CanHandle(IUpdateContext context)
        {
            if (context.IsMessageUpdate())
                return true;

            return false;
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {


            await next(context);
        }
    }
}
