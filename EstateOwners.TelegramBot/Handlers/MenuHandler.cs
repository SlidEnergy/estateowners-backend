using EstateOwners.App;
using EstateOwners.TelegramBot.Dialogs.Core;
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

            var user = await _usersService.GetByAuthTokenAsync(msg.Chat.Id.ToString(), Domain.AuthTokenType.TelegramChatId);

            if (user == null)
            {
                await _menuRenderer.ClearMenu(context);
                await next(context);
                return;
            }

            if (msg.Text == "Добавить объект недвижимости")
            {
                _dialogManager.SetActiveDialog<NewEstateDialog>(context.GetChatId().Value);
            }

            if (msg.Text == "Мои объекты недвижимости")
            {
                await context.SendEstateListAsync(context.GetChatId().Value);
            }

            await next(context);
        }
    }
}
