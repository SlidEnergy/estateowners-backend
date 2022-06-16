using EstateOwners.App.Telegram;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Slid.Security;
using System;
using System.IO;
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
        private readonly IUserSignaturesService _service;

        public AddSignatureDialog(IOptions<TelegramBotOptions> options, IUserSignaturesService service)
        {
            _options = options.Value;

            AddStep(SendGameWidget);
            AddStep(OpenExternalGameUrl);
            _service = service;
        }

        public async Task SendGameWidget(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var userSignature = await _service.GetByUserAsync(context.Store.User.Id);

            if (userSignature != null)
            {
                var image = Convert.FromBase64String(userSignature.Base64Image.Replace("data:image/webp;base64,", ""));

                using var ms = new MemoryStream(image);

                await context.Bot.Client.SendPhotoAsync(context.ChatId, new Telegram.Bot.Types.InputFiles.InputOnlineFile(ms));
            }

            await context.Bot.Client.SendGameAsync(context.ChatId, _options.DrawSignatureGameShortName,
                replyMarkup: (InlineKeyboardMarkup)ReplyMarkupBuilder.InlineKeyboard().ColumnWithCallBackGame("Создать новую подпись").ToMarkup());

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
            var encryptedPayload = Uri.EscapeUriString(Aes256.EncryptString(key, json));

            await context.Bot.Client.AnswerCallbackQueryAsync(context.Update.CallbackQuery.Id, null, false,
                Uri.EscapeUriString($"{_options.DrawSignatureUrl}draw-signature/#salt={salt}&payload={encryptedPayload}"));

            // не завершаем диалог, чтобы можно было повторно нажать на кнопку
            //context.EndDialog();
        }
    }
}
