using EstateOwners.App;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    public class MenuCommand : CommandBase
    {
        private readonly IUsersService _usersService;
        private readonly IMenuRenderer _menuRenderer;

        public MenuCommand(IUsersService usersService, IMenuRenderer menuRenderer) : base("menu")
        {
            _usersService = usersService;
            _menuRenderer = menuRenderer;
        }

        protected override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args)
        {
            var chatId = context.GetChatId().Value;

            var user = await _usersService.GetByAuthTokenAsync(chatId.ToString(), Domain.AuthTokenType.TelegramUserId);

            if (user == null)
            {
                //await _menuRenderer.ClearMenu(context);
                await next(context);
                return;
            }

            await _menuRenderer.RenderInlineMenuAsync(context);
        }
    }
}
