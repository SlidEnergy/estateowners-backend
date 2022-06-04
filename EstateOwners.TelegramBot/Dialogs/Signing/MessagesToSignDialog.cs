using EstateOwners.App.Signing;
using EstateOwners.TelegramBot.Dialogs.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs.Signing
{
    internal class MessagesToSignDialog : DialogBase<AuthDialogStore>
    {
        private readonly ISigningService _messagesToSignService;

        public MessagesToSignDialog(ISigningService messagesToSignService)
        {
            _messagesToSignService = messagesToSignService;

            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var messages = await _messagesToSignService.GetListAsync();

            foreach (var message in messages)
            {
                var signaturesCount = await _messagesToSignService.GetUserSignatureCountAsync(message.Id);

                await context.Bot.Client.ForwardMessageAsync(context.ChatId, message.FromChatId, message.MessageId);

                var buttons = new List<InlineKeyboardButton[]>();

                buttons.Add(new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Подписать", message.Id.ToString()),
                    });

                await context.Bot.Client.SendTextMessageAsync(
               context.ChatId,
               $"Подписалось уже {signaturesCount} человек, общая площадь ...",
               replyMarkup: new InlineKeyboardMarkup(buttons));
            }

            var addButtons = new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Добавить", "add"),
                }
            };

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Добавить свой документ на подпись:",
                    replyMarkup: new InlineKeyboardMarkup(addButtons));

            context.NextStep();
        }

        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            if (cq.Data == "add")
            {
                await context.ReplaceDialogAsync<AddMessageToSignDialog, AuthDialogStore>(context.Store);
                return;
            }

            await _messagesToSignService.SignAsync(context.Store.User.Id, Convert.ToInt32(cq.Data));

            await context.Bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    "Спасибо, документ подписан.");

            context.EndDialog();
        }
    }
}
