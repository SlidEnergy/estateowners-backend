using EstateOwners.TelegramBot.Dialogs.Core;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.TelegramBot.Dialogs.Signing
{
    internal class ProfileDialog : DialogBase<AuthDialogStore>
    {
        public ProfileDialog()
        {
            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var replyMarkup = ReplyMarkupBuilder.InlineKeyboard()
                .ColumnWithCallbackData("Мои объекты недвижимости", "estates")
                .RowWithCallbackData("Добавить подпись", "signature")
                .ColumnWithCallbackData("Мои машины", "cars")
                .ToMarkup();

            await context.Bot.Client.SendTextMessageAsync(context.ChatId, $"{context.Store.User}", replyMarkup: replyMarkup);

            context.NextStep();
        }

        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var cb = context.Update.CallbackQuery;

            if (cb.Data == "signature")
            {
                await context.ReplaceDialogAsync<AddSignatureDialog, AuthDialogStore>(context.Store);
                return;
            }

            if (cb.Data == "estates")
            {
                await context.ReplaceDialogAsync<EstatesDialog, AuthDialogStore>(context.Store);
                return;
            }

            if (cb.Data == "cars")
            {
                await context.ReplaceDialogAsync<CarsDialog, AuthDialogStore>(context.Store);
                return;
            }

            context.EndDialog();
        }
    }
}
