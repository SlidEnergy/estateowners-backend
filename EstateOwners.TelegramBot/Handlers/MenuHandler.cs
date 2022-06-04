using EstateOwners.App;
using EstateOwners.TelegramBot.Dialogs;
using EstateOwners.TelegramBot.Dialogs.Core;
using EstateOwners.TelegramBot.Dialogs.Polls;
using EstateOwners.TelegramBot.Dialogs.Signing;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    internal class MenuHandler : UpdateHandlerBase
    {
        private readonly IUsersService _usersService;
        private readonly IDialogManager _dialogManager;
        private readonly IMenuRenderer _menuRenderer;

        public MenuHandler(IUsersService usersService, IDialogManager dialogManager, IMenuRenderer menuRenderer)
        {
            _usersService = usersService;
            _dialogManager = dialogManager;
            _menuRenderer = menuRenderer;
        }

        public override bool CanHandle(IUpdateContext context)
        {
            if (context.IsMessageUpdate())
                return true;

            return false;
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            var msg = context.GetMessage();
            var chatId = context.GetChatId().Value;

            var user = await _usersService.GetByAuthTokenAsync(msg.Chat.Id.ToString(), Domain.AuthTokenType.TelegramUserId);

            if (user == null)
            {
                //await _menuRenderer.ClearMenu(context);
                await next(context);
                return;
            }

            var store = new AuthDialogStore(user);

            if (msg.Text == "Профиль")
            {
                _dialogManager.SetActiveDialog<ProfileDialog, AuthDialogStore>(chatId, store);
            }

            if (msg.Text == "Документы на подпись")
            {
                _dialogManager.SetActiveDialog<MessagesToSignDialog, AuthDialogStore>(chatId, store);
            }

            if (msg.Text == "Опросы")
            {
                _dialogManager.SetActiveDialog<PollsDialog, AuthDialogStore>(chatId, store);
            }

            if (msg.Text == "Председатель и совет дома")
            {
                _dialogManager.SetActiveDialog<CandidatesDialog, AuthDialogStore>(chatId, store);
            }

            if (msg.Text == "Отчетность и аудит")
            {
                _dialogManager.SetActiveDialog<EmptyDialog, DialogStore>(chatId);
            }
            await next(context);
        }
    }
}
