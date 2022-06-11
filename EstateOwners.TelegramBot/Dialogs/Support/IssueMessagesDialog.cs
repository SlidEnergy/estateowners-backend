using EstateOwners.App.Telegram.Support;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs.Support
{
    internal class IssueMessagesDialog : Dialog<AuthDialogStore>
    {
        private readonly IIssueTelegramMessagesService _service;

        public IssueMessagesDialog(IIssueTelegramMessagesService service)
        {
            _service = service;

            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var messages = await _service.GetListAsync(context.Store.User.Id);

            foreach (var message in messages)
            {
                await context.Bot.Client.ForwardMessageAsync(context.ChatId, message.FromChatId, message.MessageId);
            }

            var addButtons = new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Создать", "add"),
                }
            };

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Создать новую заявку о проблеме:",
                    replyMarkup: new InlineKeyboardMarkup(addButtons));

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            if (cq.Data == "add")
            {
                await context.ReplaceDialogAsync<AddIssueMessageDialog, AuthDialogStore>(context.Store);
                return;
            }

            context.EndDialog();
        }
    }
}
