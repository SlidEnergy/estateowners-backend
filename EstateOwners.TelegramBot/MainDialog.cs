using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    internal class MainDialog : IUpdateHandler
    {
        int step = 1;
        public static bool activate = false;

        public MainDialog()
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
                    await Step1(bot, update);
                    break;

                default:
                    throw new System.Exception("Step not supported");
            }

            return UpdateHandlingResult.Handled;
        }

        public async Task Step1(IBot bot, Update update)
        {
            var myReplyKeyboard = new ReplyKeyboardMarkup()
            {
                Keyboard = new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Сайт"),
                            new KeyboardButton("Профиль")
                        }
                    },
                ResizeKeyboard = true
            };
            await bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Добро пожаловать " + update.Message.Chat.FirstName,
                replyMarkup: myReplyKeyboard);

            step = 1;
            activate = false;
            MenuDialog.activate = true;
        }
    }
}
