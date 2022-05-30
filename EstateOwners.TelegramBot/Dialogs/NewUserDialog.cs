using EstateOwners.App;
using EstateOwners.Domain;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    internal class NewUserDialog : IUpdateHandler
    {
        int step = 1;
        public static bool activate = false;

        string _email;
        string _firstName;
        string _middleName;
        string _lastName;

        private readonly IUsersService _usersService;

        public NewUserDialog(IUsersService usersService)
        {
            step = 1;
            activate = false;
            _usersService = usersService;
        }

        public bool CanHandleUpdate(IBot bot, Update update)
        {
            return true;
        }

        public async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            if (!activate)
                return UpdateHandlingResult.Continue;

            switch (step)
            {
                case 1:
                    return await Step1(bot, update);
                case 2:
                    return await Step2(bot, update);
                case 3:
                    return await Step3(bot, update);
                case 4:
                    return await Step4(bot, update);
                case 5:
                    return await Step5(bot, update);
                case 6:
                    return await Step6(bot, update);

                default:
                    throw new System.Exception("Step not supported");
            }
        }

        public async Task<UpdateHandlingResult> Step1(IBot bot, Update update)
        {
            await bot.Client.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    "Мы вас не знаем",
                    replyMarkup: new ReplyKeyboardRemove());

            var myInlineKeyboard = new InlineKeyboardMarkup()
            {
                InlineKeyboard = new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Зарегистрироваться","register"),
                    }
                }
            };

            await bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Чтобы продолжить пользоваться, вы должны зарегестрироваться",
                replyMarkup: myInlineKeyboard);

            step++;
            return UpdateHandlingResult.Handled;
        }

        public async Task<UpdateHandlingResult> Step2(IBot bot, Update update)
        {
            CallbackQuery cq = update.CallbackQuery;

            await bot.Client.SendTextMessageAsync(
                cq.Message.Chat.Id,
                "Введите ваш email");

            step++;
            return UpdateHandlingResult.Handled;
        }

        public async Task<UpdateHandlingResult> Step3(IBot bot, Update update)
        {
            _email = update.Message.Text;

            await bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Введите вашу фамилию");

            step++;
            return UpdateHandlingResult.Handled;
        }

        public async Task<UpdateHandlingResult> Step4(IBot bot, Update update)
        {
            _lastName = update.Message.Text;

            await bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Введите ваше имя");

            step++;
            return UpdateHandlingResult.Handled;
        }

        public async Task<UpdateHandlingResult> Step5(IBot bot, Update update)
        {
            _firstName = update.Message.Text;

            await bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Введите ваше отчество");

            step++;
            return UpdateHandlingResult.Handled;
        }

        public async Task<UpdateHandlingResult> Step6(IBot bot, Update update)
        {
            _middleName = update.Message.Text;

            var result = await _usersService.CreateUserAsync(_email, update.Message.Chat.Id.ToString(), AuthTokenType.TelegramChatId);
            if (!result.Succeeded)
            {
                await bot.Client.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    "Не удалось вас зарегестрировать, попробуйте позже или свяжитесь с администратором.");

                activate = false;
                step = 1;

                return UpdateHandlingResult.Handled;
            }

            await bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Мы вас зарегестрировали");

            await bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Добавьте свой объект недвижимости. Если у вас несколько объектов, добавьте их по очереди");

            activate = false;
            step = 1;

            NewEstateDialog.activate = true;

            return UpdateHandlingResult.Continue;
        }
    }
}
