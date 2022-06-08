using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    public class MenuRenderer : IMenuRenderer
    {
        public async Task RenderMenuAsync(IUpdateContext context, CancellationToken cancellationToken = default)
        {
            var chat = context.Update.Message?.Chat ?? context.Update.CallbackQuery.Message.Chat;

            var markup = ReplyMarkupBuilder.Keyboard()
                .ColumnKeyboardButton("Документы на подпись")
                //.ColumnKeyboardButton("Опросы")
                //.NewRow()
                //.ColumnKeyboardButton("Отчетность и аудит")
                //.NewRow()
                //.ColumnKeyboardButton("Председатель и совет дома")
                .ColumnKeyboardButton("Профиль")
                .ToMarkup();

            await context.Bot.Client.SendTextMessageAsync(
                chat.Id,
                "Добро пожаловать " + chat.FirstName,
                replyMarkup: markup);
        }


        public async Task RenderInlineMenuAsync(IUpdateContext context, bool renderInlineMenu = false, CancellationToken cancellationToken = default)
        {
            var msg = context.GetMessage();

            var markup = ReplyMarkupBuilder.InlineKeyboard()
                .ColumnWithCallbackData("Документы на подпись")
                //.ColumnWithCallbackData("Опросы")
                //.NewRow()
                //.ColumnWithCallbackData("Отчетность и аудит")
                //.NewRow()
                //.ColumnWithCallbackData("Председатель и совет дома")
                .ColumnWithCallbackData("Профиль")
                .ToMarkup();

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Главное меню",
                replyMarkup: markup);
        }

        public async Task SetCommands(IUpdateContext context, CancellationToken cancellationToken = default)
        {
            await context.Bot.Client.SetMyCommandsAsync(new List<BotCommand>()
            {
                new BotCommand() { Command = "start", Description = "Начало работы"},
                new BotCommand() { Command = "menu", Description = "Главное меню"},
                new BotCommand() { Command = "documents", Description = "Документы на подпись"},
                new BotCommand() { Command = "profile", Description = "Профиль"},
            }, cancellationToken);
        }

        public async Task ClearMenuAsync(IUpdateContext context, CancellationToken cancellationToken = default)
        {
            var msg = context.GetMessage();

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Мы вас не знаем",
                replyMarkup: new ReplyKeyboardRemove());
        }

        public async Task ClearCommandsAsync(IUpdateContext context, CancellationToken cancellationToken = default)
        {
            await context.Bot.Client.SetMyCommandsAsync(new List<BotCommand>()
            {
                new BotCommand() { Command = "start", Description = "Начало работы"},
            }, cancellationToken);
        }
    }
}
