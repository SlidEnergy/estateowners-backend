using EstateOwners.App;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    internal static class IUpdateContextExtenstions
    {
        public static long? GetChatId(this IUpdateContext context)
        {
            return context.Update.Message?.Chat.Id ?? context.Update.CallbackQuery?.Message.Chat.Id;
        }

        public static async Task SendEstateListAsync(this IUpdateContext context, long chatId)
        {
            var usersService = (IUsersService)context.Services.GetService(typeof(IUsersService));
            var estatesService = (IEstatesService)context.Services.GetService(typeof(IEstatesService));

            var user = await usersService.GetByAuthTokenAsync(chatId.ToString(), Domain.AuthTokenType.TelegramUserId);

            var estates = await estatesService.GetListWithAccessCheckAsync(user.Id);

            var buttons = new InlineKeyboardButton[estates.Count][];

            for (int i = 0; i < estates.Count; i++)
            {
                var text = estates[i].ToString();

                if (text.Length > 35)
                    text = text.Substring(0, 35);

                buttons[i] = new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(text) };
            }

            await context.Bot.Client.SendTextMessageAsync(chatId, "Ваш список помещений", replyMarkup: new InlineKeyboardMarkup(buttons));
        }
    }
}
