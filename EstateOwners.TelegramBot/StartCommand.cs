using EstateOwners.App;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    public class StartCommand : CommandBase
    {
        private readonly IUsersService _usersService;

        public StartCommand(IUsersService usersService) : base("start")
        {
            _usersService = usersService;
        }

        protected override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args)
        {
            //MainDialog.activate = false;
            //NewEstateDialog.activate = false;
            //NewUserDialog.activate = false;

            var msg = context.GetMessage();

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Приветствую. Описание бота.");

            var user = await _usersService.GetByAuthTokenAsync(msg.Chat.Id.ToString(), Domain.AuthTokenType.TelegramChatId);

            if (user == null)
                await next.ReplaceDialogAsync<NewUserDialog>(context);
            else
                await next.ReplaceDialogAsync<MainDialog>(context);
        }
    }
}
