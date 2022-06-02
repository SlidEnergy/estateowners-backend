using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    internal class MainDialog : DialogBase
    {
        public override bool CanHandle(IUpdateContext context)
        {
            return true;
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            if (!activate)
            {
                await next(context);
                return;
            }

            switch (step)
            {
                case 1:
                    await Step1(context, next);
                    break;

                default:
                    throw new System.Exception("Step not supported");
            }
        }

        public async Task Step1(IUpdateContext context, UpdateDelegate next)
        {
            var msg = context.GetMessage();

            var myReplyKeyboard = new ReplyKeyboardMarkup()
            {
                Keyboard = new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Сайт"),
                            new KeyboardButton("Добавить объект недвижимости")
                        }
                    },
                ResizeKeyboard = true
            };
            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                "Добро пожаловать " + msg.Chat.FirstName,
                replyMarkup: myReplyKeyboard);

            step = 1;
            activate = false;

            await next.ReplaceDialogAsync<MenuDialog>(context);
        }
    }
}
