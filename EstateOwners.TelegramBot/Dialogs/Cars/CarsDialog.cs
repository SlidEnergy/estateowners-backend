using EstateOwners.App;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs
{
    internal class CarsDialog : Dialog<AuthDialogStore>
    {
        private readonly ICarsService _carsService;

        public CarsDialog(ICarsService carsService)
        {
            _carsService = carsService;

            AddStep(Step1);
            AddStep(Step2);
        }

        public async Task Step1(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            var cars = await _carsService.GetListWithAccessCheckAsync(context.Store.User.Id);

            var buttons = new InlineKeyboardButton[cars.Count][];

            for (int i = 0; i < buttons.Length; i++)
                buttons[i] = new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(cars[i].ToString()) };

            await context.Bot.Client.SendTextMessageAsync(context.ChatId, "Ваш список машин", replyMarkup: new InlineKeyboardMarkup(buttons));

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Добавить машину:",
                    replyMarkup: ReplyMarkupBuilder.InlineKeyboard().ColumnWithCallbackData("Добавить", "add").ToMarkup());

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<AuthDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            if (cq.Data == "add")
            {
                var store = new CarDialogStore(context.Store.User);

                await context.ReplaceDialogAsync<AddCarDialog, CarDialogStore>(store);
                return;
            }

            context.EndDialog();
        }
    }
}
