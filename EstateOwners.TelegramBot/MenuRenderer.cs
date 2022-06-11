using System;
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
        private bool _isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public async Task RenderMenuAsync(IUpdateContext context, CancellationToken cancellationToken = default)
        {
            var chat = context.Update.Message?.Chat ?? context.Update.CallbackQuery.Message.Chat;

            IReplyMarkup markup;

            if (_isDevelopment)
            {
                markup = ReplyMarkupBuilder.Keyboard()
                   .ColumnKeyboardButton("Документы на подпись")
                   .ColumnKeyboardButton("Опросы")
                   .NewRow()
                   .ColumnKeyboardButton("Председатель и совет дома")
                   .ColumnKeyboardButton("Профиль")
                   .NewRow()
                   .ColumnKeyboardButton("Помощь")
                   .ColumnKeyboardButton("Библиотека")
                   .ToMarkup();
            }
            else
            {
                markup = ReplyMarkupBuilder.Keyboard()
                    .ColumnKeyboardButton("Документы на подпись")
                    .ColumnKeyboardButton("Профиль")
                    .ToMarkup();
            }

            await context.Bot.Client.SendTextMessageAsync(
                chat.Id,
                "Добро пожаловать " + chat.FirstName,
                replyMarkup: markup);
        }


        public async Task RenderInlineMenuAsync(IUpdateContext context, bool renderInlineMenu = false, CancellationToken cancellationToken = default)
        {
            var msg = context.GetMessage();

            IReplyMarkup markup;

            if (_isDevelopment)
            {
                markup = ReplyMarkupBuilder.InlineKeyboard()
                .ColumnWithCallbackData("Документы на подпись")
                .ColumnWithCallbackData("Опросы")
                .NewRow()
                .ColumnWithCallbackData("Председатель и совет дома")
                .ColumnWithCallbackData("Профиль")
                .NewRow()
                .ColumnKeyboardButton("Помощь")
                .ColumnKeyboardButton("Библиотека")
                .ToMarkup();
            }
            else
            {
                markup = ReplyMarkupBuilder.InlineKeyboard()
              .ColumnWithCallbackData("Документы на подпись")
              .ColumnWithCallbackData("Профиль")
              .ToMarkup();
            }

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
                //new BotCommand() { Command = "library", Description = "Библиотека"},
                //new BotCommand() { Command = "support", Description = "Помощь"},
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
