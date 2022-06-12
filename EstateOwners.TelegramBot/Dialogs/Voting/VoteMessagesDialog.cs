using EstateOwners.App.Telegram.Voting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs.Voting
{
    internal class VoteMessagesDialog : Dialog<AuthDialogStore>
    {
        private readonly IVoteTelegramMessagesService _voteTelegramMessagesService;

        public VoteMessagesDialog(IVoteTelegramMessagesService messagesToSignService)
        {
            _voteTelegramMessagesService = messagesToSignService;

            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var messages = await _voteTelegramMessagesService.GetListAsync();

            foreach (var message in messages)
            {
                var statistic = await _voteTelegramMessagesService.GetUserMessageVoteCountAsync(message.Id);

                await context.Bot.Client.ForwardMessageAsync(context.ChatId, message.FromChatId, message.MessageId);

                var buttons = new List<InlineKeyboardButton[]>();

                buttons.Add(new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Подписать", message.Id.ToString()),
                    });

                await context.Bot.Client.SendTextMessageAsync(
               context.ChatId,
               $"Подписалось {statistic.UserCount} человек, {statistic.EstateCount} помещений, общая площадь {statistic.TotalArea}",
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

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            if (cq.Data == "add")
            {
                await context.ReplaceDialogAsync<AddVoteMessageDialog, AuthDialogStore>(context.Store);
                return;
            }

            await _voteTelegramMessagesService.VoteAsync(context.Store.User.Id, Convert.ToInt32(cq.Data));

            await context.Bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    "Спасибо, документ подписан.");

            context.EndDialog();
        }
    }
}
