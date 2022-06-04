using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    public class MenuRenderer : IMenuRenderer
    {
        public async Task RenderMenu(IUpdateContext context)
        {
            var msg = context.GetMessage();

            var myReplyKeyboard = new ReplyKeyboardMarkup()
            {
                Keyboard = new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Документы на подпись"),
                        new KeyboardButton("Опросы"),
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Отчетность и аудит"),
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Председатель и совет дома"),
                        new KeyboardButton("Профиль"),
                    }
                },
                ResizeKeyboard = true
            };

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Добро пожаловать " + msg.Chat.FirstName,
                replyMarkup: myReplyKeyboard);
        }

        public async Task ClearMenu(IUpdateContext context)
        {
            var msg = context.GetMessage();

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Мы вас не знаем",
                replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
