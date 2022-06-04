using EstateOwners.App.Signing;
using EstateOwners.Domain;
using EstateOwners.TelegramBot.Dialogs.Core;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.TelegramBot.Dialogs.Signing
{
    internal class AddMessageToSignDialog : DialogBase<AuthDialogStore>
    {
        private readonly ISigningService _messagesToSignService;

        public AddMessageToSignDialog(ISigningService messagesToSignService)
        {
            _messagesToSignService = messagesToSignService;

            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Пришлите сообщение или перешлите уже существующее из другого чата, которое вы хотите подписать");

            context.NextStep();
        }

        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await _messagesToSignService.AddAsync(new MessageToSign(context.Update.Message.Chat.Id, context.Update.Message.MessageId));

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Сообщение добавлено для подписи");

            context.EndDialog();
        }
    }
}
