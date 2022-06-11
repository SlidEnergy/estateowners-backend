using EstateOwners.App;
using EstateOwners.Domain;
using EstateOwners.TelegramBot.Dialogs;
using System;
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
            AddStep(Step7);
            AddStep(Step8);
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
                "Чтобы продолжить пользоваться сервисом, вы должны зарегистрироваться. Нам нужны ваши данные только для деятельности в рамках ЖК Изумрудный город." + Environment.NewLine + Environment.NewLine +
                "Продолжая пользоваться сервисом, вы даете своё согласие на хранение и обработку ваших данных.",
                replyMarkup: myInlineKeyboard);

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput)]
        public async Task Step2(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            await context.Bot.Client.SendTextMessageAsync(
                cq.Message.Chat.Id,
                "Введите ваш email (будет использоваться для связи)");

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.Message, Messages.IncorrectInput)]
        public async Task Step3(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            context.Store.Email = msg.Text;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Введите ваш номер телефона (будет использоваться для связи)");

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

            var input = $"{context.Store.LastName} {context.Store.FirstName} {context.Store.MiddleName}" + Environment.NewLine +
                $"email: {context.Store.Email}" + Environment.NewLine +
                $"телефон: {context.Store.Phone}";

            var myInlineKeyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Верно", "valid"),
                        InlineKeyboardButton.WithCallbackData("Повторить ввод данных", "retry"),
                    }
                }
            );

            await context.Bot.Client.SendTextMessageAsync(
                context.ChatId,
                "Проверьте и подтвердите введенные данные:" + Environment.NewLine + input,
                replyMarkup: myInlineKeyboard);

            context.NextStep();
        }

        [EndDialogStepFilter(UpdateType.CallbackQuery, Messages.IncorrectInput, new string[] { "valid", "retry" } )]
        public async Task Step8(DialogContext<NewUserDialogStore> context, CancellationToken cancellationToken)
        {
            var cb = context.Update.CallbackQuery;

            if (cb.Data == "retry")
            {
                await context.ExecuteStepAsync(Step2, cancellationToken);
                return;
            }

            var user = new ApplicationUser(new Trustee(), context.Store.Email)
            {
                PhoneNumber = context.Store.Phone,
                FirstName = context.Store.FirstName,
                MiddleName = context.Store.MiddleName,
                LastName = context.Store.LastName,
            };

            var result = await _usersService.CreateUserAsync(user, cb.Message.Chat.Id.ToString(), AuthTokenType.TelegramUserId);
            if (!result.Succeeded)
            {
                await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Не удалось вас зарегистрировать, попробуйте позже или свяжитесь с администратором.");

                context.EndDialog();
                return;
            }

            // ! Don't use callbackQuery.Message.From (it's bot, use callbackQuery.From)
            var telegramUser = new TelegramUser()
            {
                UserId = user.Id,
                TelegramUserId = cb.From.Id,
                FirstName = cb.From.FirstName,
                LastName = cb.From.LastName,
                Username = cb.From.Username,
                LanguageCode = cb.From.LanguageCode,
                IsBot = cb.From.IsBot
            };

            await _usersService.AddTelegramUserInfo(telegramUser);

            await context.Bot.Client.SendTextMessageAsync(
                context.ChatId,
                "Мы вас зарегистрировали");

            await _menuRenderer.RenderMenuAsync(context, cancellationToken);
            await _menuRenderer.SetCommands(context, cancellationToken);

            await context.Bot.Client.SendTextMessageAsync(
                context.ChatId,
                "Добавьте свой объект недвижимости. Если у вас несколько объектов, добавьте их по очереди");

            var store = new EstateDialogStore(user);

            await context.ReplaceDialogAsync<AddEstateDialog, EstateDialogStore>(store);
        }
    }
}
