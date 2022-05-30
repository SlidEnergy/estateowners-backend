using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace EstateOwners.TelegramBot
{
    public class CallbackQueryHandler : IUpdateHandler
    {
        public bool CanHandleUpdate(IBot bot, Update update)
        {
            if (update.Message != null)
                return true;

            return false;
        }

        public async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            //if (update.Message.Text == "/start")
            //{
            //    await bot.Client.SendTextMessageAsync(
            //                    update.Message.Chat.Id,
            //                    "Приветствую. Описание бота.");

            //    var authorized = update.Message.Chat.Id == 123;

            //    if (!authorized)
            //    {
            //        NewUserDialog.activate = true;
            //        return UpdateHandlingResult.Continue;
            //    }
            //}

            return UpdateHandlingResult.Continue;
        }
    }
}
