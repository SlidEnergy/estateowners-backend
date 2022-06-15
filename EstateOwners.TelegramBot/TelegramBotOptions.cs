using Telegram.Bot.Framework;

namespace EstateOwners.TelegramBot
{
    public class TelegramBotOptions : BotOptions
    {
        public string Aes256Key { get; set; }

        public string DrawSignatureUrl { get; set; }

        public string DrawSignatureGameShortName { get; set; }
    }
}
