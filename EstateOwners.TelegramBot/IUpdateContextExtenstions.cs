using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    internal static class IUpdateContextExtenstions
    {
        public static long? GetChatId(this IUpdateContext context)
        {
            return context.Update.Message?.Chat.Id ?? context.Update.CallbackQuery?.Message.Chat.Id;
        }
    }
}
