using EstateOwners.TelegramBot.Dialogs.Documents;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs.Documents
{
    internal class LibraryDialog : Dialog<AuthDialogStore>
    {
        private bool _isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public LibraryDialog()
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
                .ColumnWithCallbackData("Документы", "documents")
                .ColumnWithCallbackData("Отчетность и аудит", "audit")
                .ToMarkup();
            }
            else
            {
                replyMarkup = ReplyMarkupBuilder.InlineKeyboard()
                .ColumnWithCallbackData("Документы", "documents")
                .ToMarkup();
            }

            await context.Bot.Client.SendTextMessageAsync(context.ChatId, "Здесь вы можете посмотреть список сохраненных документов", replyMarkup: replyMarkup);

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var cb = context.Update.CallbackQuery;

            if (cb.Data == "documents")
            {
                await context.ReplaceDialogAsync<DocumentMessagesDialog, AuthDialogStore>(context.Store);
                return;
            }

            if (cb.Data == "audit")
            {
                await context.ReplaceDialogAsync<EmptyDialog, DialogStore>();
            }

            context.EndDialog();
        }
    }
}
