using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EstateOwners.TelegramBot.Dialogs.Estates
{
    internal class EstatesDialog : Dialog<AuthDialogStore>
    {
        public EstatesDialog()
        {
            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await context.SendEstateListAsync(context.ChatId);

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Добавить объект недвижимости:",
                    replyMarkup: ReplyMarkupBuilder.InlineKeyboard().ColumnWithCallbackData("Добавить", "add").ToMarkup());

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            if (cq.Data == "add")
            {
                var store = new EstateDialogStore(context.Store.User);

                await context.ReplaceDialogAsync<AddEstateDialog, EstateDialogStore>(store);
                return;
            }

            context.EndDialog();
        }
    }
}
