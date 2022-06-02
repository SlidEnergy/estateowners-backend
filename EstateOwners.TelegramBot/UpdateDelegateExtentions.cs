using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    internal static class UpdateDelegateExtentions
    {
        public static async Task ReplaceDialogAsync<TDialog>(this UpdateDelegate next, IUpdateContext context) where TDialog : DialogBase
        {
            UpdateDelegate newNext = context =>
            {
                var handler = (DialogBase)context.Services.GetService(typeof(TDialog));
                handler.activate = true;

                return handler.CanHandle(context) ?
                handler.HandleAsync(context, next) : next(context);
            };

            await newNext(context);
        }
    }
}
