using EstateOwners.TelegramBot.Dialogs.Estates;
using EstateOwners.TelegramBot.Dialogs.Signing;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs
{
    internal class ProfileDialog : Dialog<AuthDialogStore>
    {
        private bool _isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public ProfileDialog()
        {
            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            IReplyMarkup replyMarkup;

            if (_isDevelopment)
            {
                replyMarkup = ReplyMarkupBuilder.InlineKeyboard()
                .ColumnWithCallbackData("Мои объекты недвижимости", "estates")
                .NewRow()
                .ColumnWithCallbackData("Добавить подпись", "signature")
                .ColumnWithCallbackData("Мои машины", "cars")
                .ToMarkup();
            }
            else
            {
                replyMarkup = ReplyMarkupBuilder.InlineKeyboard()
                .ColumnWithCallbackData("Мои объекты недвижимости", "estates")
                .NewRow()
                .ColumnWithCallbackData("Добавить подпись", "signature")
                //.ColumnWithCallbackData("Мои машины", "cars")
                .ToMarkup();
            }

            await context.Bot.Client.SendTextMessageAsync(context.ChatId, $"{context.Store.User}", replyMarkup: replyMarkup);

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
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
