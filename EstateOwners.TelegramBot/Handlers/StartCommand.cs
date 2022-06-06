﻿using EstateOwners.App;
using EstateOwners.TelegramBot.Dialogs;
using EstateOwners.TelegramBot.Dialogs.Core;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    public class StartCommand : CommandBase
    {
        private readonly IUsersService _usersService;
        private readonly IDialogManager _dialogManager;
        private readonly IMenuRenderer _menuRenderer;

        public StartCommand(IUsersService usersService, IDialogManager dialogManager, IMenuRenderer menuRenderer) : base("start")
        {
            _usersService = usersService;
            _dialogManager = dialogManager;
            _menuRenderer = menuRenderer;
        }

        protected override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args)
        {
            var chatId = context.GetChatId().Value;

            await context.Bot.Client.SendTextMessageAsync(
                chatId,
                "Приветствую. Описание бота.");

            var user = await _usersService.GetByAuthTokenAsync(chatId.ToString(), Domain.AuthTokenType.TelegramUserId);

            if (user == null)
            {
                await _menuRenderer.ClearMenuAsync(context);
                await _menuRenderer.ClearCommandsAsync(context);

                _dialogManager.SetActiveDialog<NewUserDialog, NewUserDialogStore>(context.Update.Message.Chat.Id);
                await next(context);
                return;
            }
            else
            {
                await _menuRenderer.RenderMenuAsync(context);
                await _menuRenderer.SetCommands(context);
            }
        }
    }
}
