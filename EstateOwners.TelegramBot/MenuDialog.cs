using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    internal class MenuDialog : IUpdateHandler
    {
        int step = 1;
        public static bool activate = false;

        string fio;
        int liter;
        int apartament;

        public MenuDialog()
        {
            step = 1;
            activate = false;
        }

        public bool CanHandleUpdate(IBot bot, Update update)
        {
            if (update.Message != null)
                return true;

            return false;
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
            if (update.Message.Text == "Профиль")
            {
                await bot.Client.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    "Вы выбрали Профиль");
            }

            if (update.Message.Text == "Сайт")
            {
                await bot.Client.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    "Вы выбрали сайт");
            }


            step = 1;
        }
    }
}
