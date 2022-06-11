using EstateOwners.App.Telegram.Voting;
using EstateOwners.Domain.Telegram.Voting;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types.Enums;

namespace EstateOwners.TelegramBot.Dialogs.Voting
{
    internal class AddVoteMessageDialog : Dialog<AuthDialogStore>
    {
        private readonly IVoteTelegramMessagesService _voteMessagesService;

        public AddVoteMessageDialog(IVoteTelegramMessagesService messagesToSignService)
        {
            _voteMessagesService = messagesToSignService;

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

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await _voteMessagesService.AddAsync(new VoteTelegramMessage(context.Update.Message.Chat.Id, context.Update.Message.MessageId));

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Сообщение добавлено для подписи");

            context.EndDialog();
        }
    }
}
