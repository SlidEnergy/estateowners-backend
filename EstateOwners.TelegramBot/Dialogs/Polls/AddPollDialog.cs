using EstateOwners.App.Polls;
using EstateOwners.Domain;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types.Enums;

namespace EstateOwners.TelegramBot.Dialogs.Polls
{
    internal class AddPollDialog : Dialog<AuthDialogStore>
    {
        private readonly IPollsService _pollsService;

        public AddPollDialog(IPollsService pollsService)
        {
            _pollsService = pollsService;

            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Добавьте новый опрос через меню сверху справа");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await _pollsService.AddAsync(new Poll(context.ChatId, context.Update.Message.MessageId, context.Update.Message.Poll.Id));

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Опрос добавлен");

            context.EndDialog();
        }
    }
}
