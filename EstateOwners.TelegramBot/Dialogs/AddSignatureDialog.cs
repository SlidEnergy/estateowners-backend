using EstateOwners.App;
using EstateOwners.App.Signing;
using EstateOwners.TelegramBot.Dialogs.Core;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs.Signing
{
    internal class AddSignatureDialog : DialogBase<AuthDialogStore>
    {
        public AddSignatureDialog()
        {
            //AddStep(Step1);
            // AddStep(Step2);
            AddStep(SendGameWidget);
            AddStep(OpenExternalGameUrl);
            AddStep(Step3);
        }

        //public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        //{
        //    await context.Bot.Client.SendTextMessageAsync(context.ChatId, "Загрузите изображение");

        //    context.NextStep();
        //}
        //public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        //{
        //    var info = await context.Bot.Client.GetFileAsync("AgACAgIAAxkBAAIGDGKcZvT3LX3xn15qwXek9miR0MgkAAKkwTEbpR7hSCLYMJwZznQzAQADAgADbQADJAQ");

        //    using (var stream = new MemoryStream())
        //    {
        //        await context.Bot.Client.DownloadFileAsync(info.FilePath, stream);

        //    }

        //    context.EndDialog();
        //}


        public async Task SendGameWidget(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await context.Bot.Client.SendGameAsync(context.ChatId, "testgame",
                replyMarkup: (InlineKeyboardMarkup)ReplyMarkupBuilder.InlineKeyboard().ColumnWithCallbackData("Создать подпись").ToMarkup());

            context.NextStep();
        }

        public async Task OpenExternalGameUrl(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await context.Bot.Client.AnswerCallbackQueryAsync(context.Update.CallbackQuery.Id, "text", false, "https://7602-91-245-141-24.eu.ngrok.io/drawgram-static/");
        }

        public async Task Step3(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            await context.Bot.Client.SendTextMessageAsync(context.ChatId, "Ваша подпись сохранена");

            context.EndDialog();
        }
    }
}
