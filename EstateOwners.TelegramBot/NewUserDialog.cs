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

        string fio;
        int liter;
        int apartament;

        public NewUserDialog()
        {
            step = 1;
            activate = false;
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
                "Введите ваше ФИО");

            step++;
            return UpdateHandlingResult.Handled;
        }

        public async Task<UpdateHandlingResult> Step3(IBot bot, Update update)
        {
            fio = update.Message.Text;

            var myInlineKeyboard = new InlineKeyboardMarkup()
            {
                InlineKeyboard = new InlineKeyboardButton[][]
                 {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("1"),
                        InlineKeyboardButton.WithCallbackData("2"),
                        InlineKeyboardButton.WithCallbackData("3"),
                        InlineKeyboardButton.WithCallbackData("4"),
                        InlineKeyboardButton.WithCallbackData("5"),
                        InlineKeyboardButton.WithCallbackData("6"),
                    }
                 }
            };

            await bot.Client.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    "В каком литере у вас недвижимость. Если у вас несколько объектов, добавьте их по очереди",
                    replyMarkup: myInlineKeyboard);

            step++;
            return UpdateHandlingResult.Handled;
        }

        public async Task<UpdateHandlingResult> Step4(IBot bot, Update update)
        {
            CallbackQuery cq = update.CallbackQuery;

            liter = Convert.ToInt32(cq.Data);

            await bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    "Введите номер апартамента");

            step++;

            return UpdateHandlingResult.Handled;
        }

        public async Task<UpdateHandlingResult> Step5(IBot bot, Update update)
        {
            apartament = Convert.ToInt32(update.Message.Text);

            await bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Ваши данные: " + fio + " " + liter + "-" + apartament);

            activate = false;
            step = 1;
            StartCommand.authorized = true;

            MainDialog.activate = true;

            return UpdateHandlingResult.Continue;
        }
    }
}
