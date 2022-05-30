using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EstateOwners.TelegramBot
{
    public class EstateOwnersBot : BotBase<EstateOwnersBot>
    {
        public EstateOwnersBot(IOptions<BotOptions<EstateOwnersBot>> botOptions)
            : base(botOptions) { }

        public override async Task HandleUnknownUpdate(Update update)
        {
           // _logger.LogWarning("Unable to handle an update");

            const string unknownUpdateText = "Sorry! I don't know what to do with this message";

            if (update.Type == UpdateType.MessageUpdate)
            {
                await Client.SendTextMessageAsync(update.Message.Chat.Id,
                    unknownUpdateText,
                    replyToMessageId: update.Message.MessageId);
            }
            else
            {

            }
        }

        public override Task HandleFaultedUpdate(Update update, Exception e)
        {
            //_logger.LogCritical("Exception thrown while handling an update");
            return Task.CompletedTask;
        }
    }
}
