using EstateOwners.App;
using EstateOwners.App.Signing;
using EstateOwners.Domain;
using EstateOwners.TelegramBot.Dialogs.Core;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot.Dialogs.Signing
{
    internal class AddSignatureDialog : DialogBase
    {
        private readonly ISigningService _messagesToSignService;
        private readonly IUsersService _usersService;

        public AddSignatureDialog(ISigningService messagesToSignService, IUsersService usersService)
        {
            _messagesToSignService = messagesToSignService;
            _usersService = usersService;

            AddStep(Step1);
            AddStep(Step2);
            AddStep(Step3);
        }

        public async Task Step1(DialogContext context, CancellationToken cancellationToken)
        {
            var msg = context.GetMessage();

            var chatId = msg?.Chat.Id ?? context.Update.CallbackQuery.Message.Chat.Id;

            var user = await _usersService.GetByAuthTokenAsync(chatId.ToString(), AuthTokenType.TelegramChatId);

            if (user == null)
            {
                await context.ReplaceDialogAsync<NewUserDialog>();
                return;
            }

            context.Values["user"] = user;

            //await context.Bot.Client.SendTextMessageAsync(chatId, "game", replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            //{
            //    InlineKeyboardButton.WithCallBackGame("play", new Telegram.Bot.Types.CallbackGame() {
            //        {
                    
            //        })
            //    }
            //}))

            await context.Bot.Client.SendGameAsync(chatId, "testgame");

            context.NextStep();
        }

        public async Task Step2(DialogContext context, CancellationToken cancellationToken)
        {
            await context.Bot.Client.AnswerCallbackQueryAsync(context.Update.CallbackQuery.Id, "text", false, "https://7602-91-245-141-24.eu.ngrok.io/drawgram-static/");
        }

        public async Task Step3(DialogContext context, CancellationToken cancellationToken)
        {
            await context.Bot.Client.SendTextMessageAsync(context.ChatId, "Ваша подпись сохранена");

            context.EndDialog();
        }
    }
}
