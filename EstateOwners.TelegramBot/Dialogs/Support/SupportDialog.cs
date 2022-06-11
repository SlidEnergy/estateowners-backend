using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs.Support
{
    internal class SupportDialog : Dialog<AuthDialogStore>
    {
        private bool _isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public SupportDialog()
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
                .ColumnWithCallbackData("Справка", "help")
                .ColumnWithCallbackData("Сообщить о проблеме", "issues")
                .ToMarkup();
            }
            else
            {
                replyMarkup = ReplyMarkupBuilder.InlineKeyboard()
                .ColumnWithCallbackData("Справка", "help")
                .ColumnWithCallbackData("Сообщить о проблеме", "issues")
                .ToMarkup();
            }

            await context.Bot.Client.SendTextMessageAsync(context.ChatId, "Здесь вы можете посмотреть ваши заявки о проблемах и создать новую.", replyMarkup: replyMarkup);

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var cb = context.Update.CallbackQuery;

            if (cb.Data == "issues")
            {
                await context.ReplaceDialogAsync<IssueMessagesDialog, AuthDialogStore>(context.Store);
                return;
            }

            if (cb.Data == "help")
            {
                await context.Bot.Client.SendTextMessageAsync(context.ChatId, 
                    @"Раздел Документы на подпись - здесь вы ставите подпись под документом, который активисты отнесут в нужную инстанцию." + Environment.NewLine +
                    @"Раздел Профиль - здесь вы можете отредактировать свои объекты недвижимости." + Environment.NewLine +
                    @"Раздел Помощь - здесь вы отправить сообщение о проблеме и посмотреть свои прошлые заявки." + Environment.NewLine +
                    @"Раздел Библиотека - здесь вы можете ознакомиться с прикрепленными документами и скачать их");

            }

            context.EndDialog();
        }
    }
}
