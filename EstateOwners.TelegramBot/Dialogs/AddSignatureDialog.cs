using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Slid.Security;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs.Signing
{
    internal class AddSignatureDialog : Dialog<AuthDialogStore>
    {
        private readonly TelegramBotOptions _options;

        public AddSignatureDialog(IOptions<TelegramBotOptions> options)
        {
            _options = options.Value;

            AddStep(SendGameWidget);
            AddStep(OpenExternalGameUrl);
        }

        public async Task SendGameWidget(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await context.Bot.Client.SendGameAsync(context.ChatId, _options.DrawUserSignatureGameShortName,
                replyMarkup: (InlineKeyboardMarkup)ReplyMarkupBuilder.InlineKeyboard().ColumnWithCallBackGame("Создать подпись").ToMarkup());

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task OpenExternalGameUrl(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var payload = new TelegramGamePayload
            {
                MessageId = context.Update.CallbackQuery.Message.MessageId,
                ChatId = context.ChatId,
            };

            var json = JsonConvert.SerializeObject(payload);

            var salt = ((int)DateTime.Now.TimeOfDay.TotalSeconds).ToString();
            var key = _options.Aes256Key.Substring(0, _options.Aes256Key.Length - salt.Length) + salt;
            var encryptedPayload = Uri.EscapeDataString(Aes256.EncryptString(key, json));

            await context.Bot.Client.AnswerCallbackQueryAsync(context.Update.CallbackQuery.Id, null, false,
                Uri.EscapeDataString($"{_options.DrawUserSignatureUrl}draw-user-signature/#salt={salt}&payload={encryptedPayload}"));

            context.EndDialog();
        }
    }
}
