using EstateOwners.App;
using EstateOwners.App.Signing;
using EstateOwners.Domain;
using EstateOwners.TelegramBot.Dialogs.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs
{
    internal class CandidatesDialog : DialogBase
    {
        private readonly ICandidatesService _candidatesService;
        private readonly IUsersService _usersService;

        public CandidatesDialog(ICandidatesService candidatesService, IUsersService usersService)
        {
            _candidatesService = candidatesService;
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
                   chatId,
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
                    chatId,
                    "Добавить свою кандидатуру:",
                    replyMarkup: new InlineKeyboardMarkup(addButtons));

            context.NextStep();
        }

        public async Task Step2(DialogContext context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            var user = (ApplicationUser)context.Values["user"];

            if (cq.Data == "chairman" || cq.Data == "boardMember")
            {
                if (cq.Data == "chairman")
                {
                    await _candidatesService.AddAsync(user.Id, CandidateType.Chairman);
                }

                if (cq.Data == "boardMember")
                {
                    await _candidatesService.AddAsync(user.Id, CandidateType.BoardMember);
                }

                await context.Bot.Client.SendTextMessageAsync(
                   cq.Message.Chat.Id,
                   "Ваша кандидатура добавлена.");

                context.EndDialog();
                return;
            }

            await _candidatesService.VoteForCandidateAsync(user.Id, Convert.ToInt32(cq.Data));

            await context.Bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    "Спасибо, ваш голос учтен.");

            context.EndDialog();
        }
    }
}
