using AutoMapper;
using EstateOwners.App;
using EstateOwners.Domain;
using EstateOwners.TelegramBot.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Dialogs;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    internal class NewUserDialog : Dialog<NewUserDialogStore>
    {
        private readonly IUsersService _usersService;
        private readonly IMenuRenderer _menuRenderer;
        private readonly IMapper _mapper;

        public NewUserDialog(IUsersService usersService, IMenuRenderer menuRenderer, IMapper mapper)
        {
            _usersService = usersService;
            _menuRenderer = menuRenderer;
            _mapper = mapper;

            AddStep(Step1);
            AddStep(Step2);
            AddStep(Step3);
            AddStep(Step4);
            AddStep(Step5);
            AddStep(Step6);
            AddStep(Step7);
        }

        public async Task Step1(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            //await _menuRenderer.ClearMenu(context);

            var myInlineKeyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Зарегистрироваться","register"),
                    }
                }
            );

            await context.Bot.Client.SendTextMessageAsync(
                context.ChatId,
                "Чтобы продолжить пользоваться сервисом, вы должны зарегистрироваться. Продолжая пользоваться сервисом, вы даете своё согласие на хранение и обработку ваших данных. Сейчас нам нужны ваши данные только для составления списка собственников, для распечатки подписавшихся под документом.",
                replyMarkup: myInlineKeyboard);

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            await context.Bot.Client.SendTextMessageAsync(
                cq.Message.Chat.Id,
                "Введите ваш email");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step3(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.Email = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Введите ваш номер телефона");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step4(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.Phone = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Введите вашу фамилию");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step5(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.LastName = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Введите ваше имя");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step6(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.FirstName = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Введите ваше отчество");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step7(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.MiddleName = msg.Text;

            var user = new ApplicationUser(new Trustee(), context.Store.Email)
            {
                PhoneNumber = context.Store.Phone,
                FirstName = context.Store.FirstName,
                MiddleName = context.Store.MiddleName,
                LastName = context.Store.LastName,
            };

            var result = await _usersService.CreateUserAsync(user, msg.From.Id.ToString(), AuthTokenType.TelegramUserId);
            if (!result.Succeeded)
            {
                await context.Bot.Client.SendTextMessageAsync(
                    msg.Chat.Id,
                    "Не удалось вас зарегистрировать, попробуйте позже или свяжитесь с администратором.");

                context.EndDialog();
                return;
            }

            var telegramUser = new TelegramUser()
            {
                UserId = user.Id,
                TelegramUserId = msg.From.Id,
                FirstName = msg.From.FirstName,
                LastName = msg.From.LastName,
                Username = msg.From.Username,
                LanguageCode = msg.From.LanguageCode,
                IsBot = msg.From.IsBot
            };

            await _usersService.AddTelegramUserInfo(telegramUser);

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Мы вас зарегистрировали");

            await _menuRenderer.RenderMenuAsync(context, cancellationToken);
            await _menuRenderer.SetCommands(context, cancellationToken);

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Добавьте свой объект недвижимости. Если у вас несколько объектов, добавьте их по очереди");

            var store = new EstateDialogStore(user);

            await context.ReplaceDialogAsync<AddEstateDialog, EstateDialogStore>(store);
        }
    }
}
