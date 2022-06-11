using EstateOwners.App.Telegram.Documents;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs.Documents
{
    internal class DocumentMessagesDialog : Dialog<AuthDialogStore>
    {
        private readonly IDocumentTelegramMessagesService _service;

        public DocumentMessagesDialog(IDocumentTelegramMessagesService service)
        {
            _service = service;

            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var messages = await _service.GetListAsync();

            foreach (var message in messages)
            {
                await context.Bot.Client.ForwardMessageAsync(context.ChatId, message.FromChatId, message.MessageId);
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
                    "Добавить документ:",
                    replyMarkup: new InlineKeyboardMarkup(addButtons));

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            if (cq.Data == "add")
            {
                await context.ReplaceDialogAsync<AddDocumentMessageDialog, AuthDialogStore>(context.Store);
                return;
            }

            context.EndDialog();
        }
    }
}
