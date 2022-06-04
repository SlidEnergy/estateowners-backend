using EstateOwners.App;
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
            var msg = context.GetMessage();

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Приветствую. Описание бота.");

            var user = await _usersService.GetByAuthTokenAsync(msg.Chat.Id.ToString(), Domain.AuthTokenType.TelegramChatId);

            if (user == null)
            {
                await _menuRenderer.ClearMenu(context);

                _dialogManager.SetActiveDialog<NewUserDialog, NewUserDialogStore>(context.Update.Message.Chat.Id);
            }
            else
            {
                await _menuRenderer.RenderMenu(context);
            }

            await next(context);
        }
    }
}
