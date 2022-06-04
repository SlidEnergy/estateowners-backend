using EstateOwners.App;
using EstateOwners.Domain;
using EstateOwners.TelegramBot.Dialogs;
using EstateOwners.TelegramBot.Dialogs.Core;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    internal class NewUserDialog : DialogBase<NewUserDialogStore>
    {
        private readonly IUsersService _usersService;
        private readonly IMenuRenderer _menuRenderer;

        public NewUserDialog(IUsersService usersService, IMenuRenderer menuRenderer)
        {
            _usersService = usersService;
            _menuRenderer = menuRenderer;

            AddStep(Step1);
            AddStep(Step2);
            AddStep(Step3);
            AddStep(Step4);
            AddStep(Step5);
            AddStep(Step6);
        }

        public async Task Step1(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            await _menuRenderer.ClearMenu(context);

            var myInlineKeyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Зарегистрироваться","register"),
                    }
                }
            );

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Чтобы продолжить пользоваться, вы должны зарегестрироваться",
                replyMarkup: myInlineKeyboard);

            context.NextStep();
        }

        public async Task Step2(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            await context.Bot.Client.SendTextMessageAsync(
                cq.Message.Chat.Id,
                "Введите ваш email");

            context.NextStep();
        }

        public async Task Step3(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.Email = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Введите вашу фамилию");

            context.NextStep();
        }

        public async Task Step4(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.LastName = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Введите ваше имя");

            context.NextStep();
        }

        public async Task Step5(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.FirstName = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Введите ваше отчество");

            context.NextStep();
        }

        public async Task Step6(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            var user = new ApplicationUser(new Trustee(), context.Store.Email)
            {
                FirstName = context.Store.FirstName,
                MiddleName = context.Store.MiddleName,
                LastName = context.Store.LastName,
            };

            var result = await _usersService.CreateUserAsync(user, msg.Chat.Id.ToString(), AuthTokenType.TelegramChatId);
            if (!result.Succeeded)
            {
                await context.Bot.Client.SendTextMessageAsync(
                    msg.Chat.Id,
                    "Не удалось вас зарегестрировать, попробуйте позже или свяжитесь с администратором.");

                context.EndDialog();
                return;
            }

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Мы вас зарегестрировали");

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Добавьте свой объект недвижимости. Если у вас несколько объектов, добавьте их по очереди");

            var store = new EstateDialogStore(user);

            await context.ReplaceDialogAsync<AddEstateDialog, EstateDialogStore>(store);
        }
    }
}
