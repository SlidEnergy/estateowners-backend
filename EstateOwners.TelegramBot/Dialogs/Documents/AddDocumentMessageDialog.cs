using EstateOwners.App.Telegram.Documents;
using EstateOwners.Domain.Telegram;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types.Enums;

namespace EstateOwners.TelegramBot.Dialogs.Documents
{
    internal class AddDocumentMessageDialog : Dialog<AuthDialogStore>
    {
        private readonly IDocumentTelegramMessagesService _service;

        public AddDocumentMessageDialog(IDocumentTelegramMessagesService service)
        {
            _service = service;

            AddStep(Step1);
            AddStep(Step2);
            AddStep(Step3);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Пришлите сообщение с документом или перешлите уже существующее из другого чата");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await _service.AddAsync(new DocumentTelegramMessage(context.Update.Message.Chat.Id, context.Update.Message.MessageId));

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Документ добавлен");

            var replyMarkup = ReplyMarkupBuilder.InlineKeyboard()
                .ColumnWithCallbackData("Добавить", "add")
                .ColumnWithCallbackData("Нет", "no")
                .ToMarkup();

            await context.Bot.Client.SendTextMessageAsync(
                context.ChatId,
                "Хотите добавить еще документ?",
                replyMarkup: replyMarkup);

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step3(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var cb = context.Update.CallbackQuery;

            if (cb.Data == "add")
            {
                await context.ExecuteStepAsync(Step1, cancellationToken);
                return;
            }

            await context.Bot.Client.SendTextMessageAsync(
                cb.Message.Chat.Id,
                "Добавление завершено");

            context.EndDialog();
        }
    }
}
