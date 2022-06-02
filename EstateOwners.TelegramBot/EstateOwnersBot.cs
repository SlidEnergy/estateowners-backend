using Microsoft.Extensions.Options;
using Telegram.Bot.Framework;

namespace EstateOwners.TelegramBot
{
    public class EstateOwnersBot : BotBase
    {
        public EstateOwnersBot(IOptions<BotOptions> botOptions)
            : base(botOptions.Value) { }
    }
}
