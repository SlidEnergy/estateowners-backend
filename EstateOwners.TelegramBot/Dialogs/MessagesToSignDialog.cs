using EstateOwners.App;
using EstateOwners.Domain;
using EstateOwners.TelegramBot.Dialogs.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    internal class MessagesToSignDialog : DialogBase
    {
        private readonly ISigningService _messagesToSignService;
        private readonly IUsersService _usersService;

        public MessagesToSignDialog(ISigningService messagesToSignService, IUsersService usersService)
        {
            _messagesToSignService = messagesToSignService;
            _usersService = usersService;

            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            var chatId = msg?.Chat.Id ?? context.Update.CallbackQuery.Message.Chat.Id;

            var user = await _usersService.GetByAuthTokenAsync(chatId.ToString(), AuthTokenType.TelegramChatId);

            if (user == null)
            {
                await context.ReplaceDialogAsync<NewUserDialog>();
                return;
            }

            context.Values["user"] = user;

            var messages = await _messagesToSignService.GetListAsync();

            foreach (var message in messages)
            {
                var signaturesCount = await _messagesToSignService.GetUserSignatureCountAsync(message.Id);

                await context.Bot.Client.ForwardMessageAsync(chatId, message.FromChatId, message.MessageId);

                var buttons = new List<InlineKeyboardButton[]>();

                buttons.Add(new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Подписать", message.Id.ToString()),
                    });

                await context.Bot.Client.SendTextMessageAsync(
               chatId,
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
                    chatId,
                    "Добавить свой документ на подпись:",
                    replyMarkup: new InlineKeyboardMarkup(addButtons));

            context.NextStep();
        }

        public async Task Step2(DialogContext context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            if (cq.Data == "add")
            {
                await context.ReplaceDialogAsync<AddMessageToSignDialog>();
                return;
            }

            var user = (ApplicationUser)context.Values["user"];

            await _messagesToSignService.SignAsync(user.Id, Convert.ToInt32(cq.Data));

            await context.Bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    "Спасибо, документ подписан.");

            context.EndDialog();
        }
    }
}
