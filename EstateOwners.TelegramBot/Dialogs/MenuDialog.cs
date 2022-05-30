using EstateOwners.App;
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
        public static bool activate = true;

        string fio;
        int liter;
        int apartament;

        private readonly IUsersService _usersService;

        public MenuDialog(IUsersService usersService)
        {
            step = 1;
            activate = false;
            _usersService = usersService;
        }

        public bool CanHandleUpdate(IBot bot, Update update)
        {
            if (update.Message != null)
                return true;

            return false;
        }

        public async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            var user = await _usersService.GetByAuthTokenAsync(update.Message.Chat.Id.ToString(), Domain.AuthTokenType.TelegramChatId);

            if (user == null)
            {
                activate = true;
                return UpdateHandlingResult.Continue;
            }

            switch (step)
            {
                case 1:
                    return await Step1(bot, update);

                default:
                    throw new System.Exception("Step not supported");
            }
        }

        public async Task<UpdateHandlingResult> Step1(IBot bot, Update update)
        {
            if (update.Message.Text == "Добавить объект недвижимости")
            {
                step = 1;
                NewEstateDialog.activate = true;
                return UpdateHandlingResult.Continue;
            }

            if (update.Message.Text == "Сайт")
            {
                await bot.Client.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    "Вы выбрали сайт");
            }


            step = 1;
            return UpdateHandlingResult.Continue;
        }
    }
}
