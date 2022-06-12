using EstateOwners.App;
using EstateOwners.TelegramBot.Dialogs;
using EstateOwners.TelegramBot.Dialogs.Documents;
using EstateOwners.TelegramBot.Dialogs.Polls;
using EstateOwners.TelegramBot.Dialogs.Support;
using EstateOwners.TelegramBot.Dialogs.Voting;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Framework.Dialogs;

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
            if (context.IsMessageUpdate() || context.IsCallbackQueryUpdate())
                return true;

            return false;
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            var msg = context.Update.Message?.Text ?? context.Update.CallbackQuery?.Data;

            if (msg == null)
            {
                await next(context);
                return;
            }

            var chatId = context.GetChatId().Value;

            var user = await _usersService.GetByAuthTokenAsync(chatId.ToString(), Domain.AuthTokenType.TelegramUserId);

            if (user == null)
            {
                //await _menuRenderer.ClearMenu(context);
                await next(context);
                return;
            }

            var store = new AuthDialogStore(user);

            if (msg == "Профиль" || msg == "/profile")
            {
                _dialogManager.SetActiveDialog<ProfileDialog, AuthDialogStore>(chatId, store);
            }

            if (msg == "Документы на подпись" || msg == "/documents")
            {
                _dialogManager.SetActiveDialog<VoteMessagesDialog, AuthDialogStore>(chatId, store);
            }

            if (msg == "Опросы")
            {
                _dialogManager.SetActiveDialog<PollsDialog, AuthDialogStore>(chatId, store);
            }

            if (msg == "Председатель и совет дома")
            {
                _dialogManager.SetActiveDialog<CandidatesDialog, AuthDialogStore>(chatId, store);
            }

            if (msg == "Помощь" || msg == "/help")
            {
                _dialogManager.SetActiveDialog<SupportDialog, AuthDialogStore>(chatId, store);
            }

            if (msg == "Библиотека" || msg == "/llibrary")
            {
                _dialogManager.SetActiveDialog<LibraryDialog, AuthDialogStore>(chatId, store);
            }

            await next(context);
        }
    }
}
