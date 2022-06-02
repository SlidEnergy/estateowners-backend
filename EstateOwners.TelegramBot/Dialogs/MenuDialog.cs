using EstateOwners.App;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    internal class MenuDialog : DialogBase
    {
        private readonly IUsersService _usersService;

        public MenuDialog(IUsersService usersService)
        {
            _usersService = usersService;
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
                activate = true;
                await next(context);
                return;
            }

            switch (step)
            {
                case 1:
                    await Step1(context, next);
                    break;

                default:
                    throw new System.Exception("Step is not supported");
            }
        }

        public async Task Step1(IUpdateContext context, UpdateDelegate next)
        {
            var msg = context.GetMessage();

            if (msg.Text == "Добавить объект недвижимости")
            {
                step = 1;
                await next.ReplaceDialogAsync<NewEstateDialog>(context);
                return;
            }

            if (msg.Text == "Сайт")
            {
                await context.Bot.Client.SendTextMessageAsync(
                    msg.Chat.Id,
                    "Вы выбрали сайт");
            }

            step = 1;
            await next(context);
        }
    }
}
