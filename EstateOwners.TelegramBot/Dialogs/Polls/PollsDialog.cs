using EstateOwners.App.Polls;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs.Polls
{
    internal class PollsDialog : Dialog<AuthDialogStore>
    {
        private readonly IPollsService _pollsService;

        public PollsDialog(IPollsService pollsService)
        {
            _pollsService = pollsService;

            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var polls = await _pollsService.GetListAsync();

            foreach (var poll in polls)
            {
                await context.Bot.Client.ForwardMessageAsync(context.ChatId, poll.FromChatId, poll.MessageId);
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
                    "Добавить свой опрос:",
                    replyMarkup: new InlineKeyboardMarkup(addButtons));

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            if (cq.Data == "add")
            {
                await context.ReplaceDialogAsync<AddPollDialog, AuthDialogStore>(context.Store);
                return;
            }

            context.EndDialog();
        }
    }
}
