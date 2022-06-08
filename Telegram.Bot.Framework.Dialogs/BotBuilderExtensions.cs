using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework.Dialogs
{
    public static class BotBuilderExtensions
    {
        public static IBotBuilder UseDialogHandler(this IBotBuilder builder)
        {
            builder.Use<DialogHandler>();

            return builder;
        }
    }
}
