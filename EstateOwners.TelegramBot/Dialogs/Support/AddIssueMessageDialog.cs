using EstateOwners.App.Telegram.Support;
using EstateOwners.Domain.Telegram.Support;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types.Enums;

namespace EstateOwners.TelegramBot.Dialogs.Support
{
    internal class AddIssueMessageDialog : Dialog<AuthDialogStore>
    {
        private readonly IIssueTelegramMessagesService _service;

        public AddIssueMessageDialog(IIssueTelegramMessagesService service)
        {
            _service = service;

            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Пришлите сообщение с вопросом или перешлите уже существующее из другого чата");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await _service.AddAsync(new IssueTelegramMessage(context.Store.User.Id, context.Update.Message.Chat.Id, context.Update.Message.MessageId));

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Вопрос добавлен");

            context.EndDialog();
        }
    }
}
