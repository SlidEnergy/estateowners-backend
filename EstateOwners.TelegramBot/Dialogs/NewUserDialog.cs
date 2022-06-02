using EstateOwners.App;
using EstateOwners.Domain;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    internal class NewUserDialog : DialogBase
    {
        string _email;
        string _firstName;
        string _middleName;
        string _lastName;

        private readonly IUsersService _usersService;

        public NewUserDialog(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public override bool CanHandle(IUpdateContext context)
        {
            return true;
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            if (!activate)
            {
                await next(context);
                return;
            }

            switch (step)
            {
                case 1:
                    await Step1(context, next);
                    break;
                case 2:
                    await Step2(context, next);
                    break;
                case 3:
                    await Step3(context, next);
                    break;
                case 4:
                    await Step4(context, next);
                    break;
                case 5:
                    await Step5(context, next);
                    break;
                case 6:
                    await Step6(context, next);
                    break;

                default:
                    throw new System.Exception("Step not supported");
            }
        }

        public async Task Step1(IUpdateContext context, UpdateDelegate next)
        {
            var msg = context.GetMessage();

            await context.Bot.Client.SendTextMessageAsync(
                    msg.Chat.Id,
                    "Мы вас не знаем",
                    replyMarkup: new ReplyKeyboardRemove());

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

            step++;
        }

        public async Task Step2(IUpdateContext context, UpdateDelegate next)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            await context.Bot.Client.SendTextMessageAsync(
                cq.Message.Chat.Id,
                "Введите ваш email");

            step++;
        }

        public async Task Step3(IUpdateContext context, UpdateDelegate next)
        {
            var msg = context.GetMessage();

            _email = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Введите вашу фамилию");

            step++;
        }

        public async Task Step4(IUpdateContext context, UpdateDelegate next)
        {
            var msg = context.GetMessage();

            _lastName = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Введите ваше имя");

            step++;
        }

        public async Task Step5(IUpdateContext context, UpdateDelegate next)
        {
            var msg = context.GetMessage();

            _firstName = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Введите ваше отчество");

            step++;
        }

        public async Task Step6(IUpdateContext context, UpdateDelegate next)
        {
            var msg = context.GetMessage();

            _middleName = msg.Text;

            var user = new ApplicationUser(new Trustee(), _email)
            {
                FirstName = _firstName,
                MiddleName = _middleName,
                LastName = _lastName
            };

            var result = await _usersService.CreateUserAsync(_email, msg.Chat.Id.ToString(), AuthTokenType.TelegramChatId);
            if (!result.Succeeded)
            {
                await context.Bot.Client.SendTextMessageAsync(
                    msg.Chat.Id,
                    "Не удалось вас зарегестрировать, попробуйте позже или свяжитесь с администратором.");

                activate = false;
                step = 1;

                return;
            }

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Мы вас зарегестрировали");

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Добавьте свой объект недвижимости. Если у вас несколько объектов, добавьте их по очереди");

            activate = false;
            step = 1;

            await next.ReplaceDialogAsync<NewEstateDialog>(context);
        }
    }
}
