using EstateOwners.App.Signing;
using EstateOwners.Domain;
using EstateOwners.TelegramBot.Dialogs.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs
{
    internal class CandidatesDialog : DialogBase<AuthDialogStore>
    {
        private readonly ICandidatesService _candidatesService;

        public CandidatesDialog(ICandidatesService candidatesService)
        {
            _candidatesService = candidatesService;

            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var candidates = await _candidatesService.GetListAsync();

            foreach (var candidate in candidates)
            {
                var voteCount = await _candidatesService.GetVotesForCandidatesCount(candidate.Id);

                var buttons = new List<InlineKeyboardButton[]>();

                buttons.Add(new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Проголосовать", candidate.Id.ToString()),
                    });

                await context.Bot.Client.SendTextMessageAsync(
                   context.ChatId,
                   $"{candidate} - {voteCount} голосов",
                   replyMarkup: new InlineKeyboardMarkup(buttons));
            }

            var addButtons = new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Председатель", "chairman"),
                    InlineKeyboardButton.WithCallbackData("Член совета дома", "boardMember"),
                }
            };

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Добавить свою кандидатуру:",
                    replyMarkup: new InlineKeyboardMarkup(addButtons));

            context.NextStep();
        }

        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            if (cq.Data == "chairman" || cq.Data == "boardMember")
            {
                if (cq.Data == "chairman")
                {
                    await _candidatesService.AddAsync(context.Store.User.Id, CandidateType.Chairman);
                }

                if (cq.Data == "boardMember")
                {
                    await _candidatesService.AddAsync(context.Store.User.Id, CandidateType.BoardMember);
                }

                await context.Bot.Client.SendTextMessageAsync(
                   cq.Message.Chat.Id,
                   "Ваша кандидатура добавлена.");

                context.EndDialog();
                return;
            }

            await _candidatesService.VoteForCandidateAsync(context.Store.User.Id, Convert.ToInt32(cq.Data));

            await context.Bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    "Спасибо, ваш голос учтен.");

            context.EndDialog();
        }
    }
}
