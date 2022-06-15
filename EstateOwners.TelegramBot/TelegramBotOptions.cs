using Telegram.Bot.Framework;

namespace EstateOwners.TelegramBot
{
    public class TelegramBotOptions : BotOptions
    {
        public string Aes256Key { get; set; }

        public string DrawUserSignatureUrl { get; set; }

        public string DrawUserSignatureGameShortName { get; set; }
    }
}
