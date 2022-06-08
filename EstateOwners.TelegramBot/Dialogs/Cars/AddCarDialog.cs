using EstateOwners.App;
using EstateOwners.Domain;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs
{
    internal class AddCarDialog : Dialog<CarDialogStore>
    {
        private readonly ICarsService _carsService;

        public AddCarDialog(ICarsService carsService)
        {
            _carsService = carsService;

            AddStep(Step1);
            AddStep(Step2);
            AddStep(Step3);
            AddStep(Step4);
            AddStep(Step5);
        }

        public async Task Step1(DialogContext<CarDialogStore> context, CancellationToken cancellationToken)
        {
            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Введите марку вашей машины");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<CarDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.Title = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Введите цвет вашей машины");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step3(DialogContext<CarDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.Color = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Введите номер вашей машины");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step4(DialogContext<CarDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.Number = msg.Text;

            var model = new Car(context.Store.Number)
            {
                Color = context.Store.Color,
                Title = context.Store.Title
            };

            var car = await _carsService.AddAsync(context.Store.User.Id, model);

            if (car == null)
            {
                await context.Bot.Client.SendTextMessageAsync(
                    msg.Chat.Id,
                    "Не удалось добавить машину, попробуйте позже или обратитесь к администратору.");

                context.EndDialog();
                return;
            }

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Ваша машина добавлена.");

            var myInlineKeyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Добавить", "add"),
                        InlineKeyboardButton.WithCallbackData("Нет", "no"),
                    }
                }
            );

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Хотите добавить еще машину?",
                replyMarkup: myInlineKeyboard);

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step5(DialogContext<CarDialogStore> context, CancellationToken cancellationToken)
        {
            var cb = context.Update.CallbackQuery;

            if (cb.Data == "add")
            {
                await context.ExecuteStepAsync(Step1, cancellationToken);
                return;
            }

            await context.Bot.Client.SendTextMessageAsync(
                cb.Message.Chat.Id,
                "Добавление завершено");

            context.EndDialog();
        }
    }
}
