using EstateOwners.App;
using EstateOwners.Domain;
using EstateOwners.TelegramBot.Dialogs.Core;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;

namespace EstateOwners.TelegramBot
{
    internal class AddMessageToSignDialog : DialogBase
    {
        private readonly ISigningService _messagesToSignService;
        private readonly IUsersService _usersService;

        public AddMessageToSignDialog(ISigningService messagesToSignService, IUsersService usersService)
        {
            _messagesToSignService = messagesToSignService;
            _usersService = usersService;

            AddStep(Step1);
            AddStep(Step2);
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

            await context.Bot.Client.SendTextMessageAsync(
                    chatId,
                    "Пришлите сообщение или перешлите уже существующее из другого чата, которое вы хотите подписать");

            context.NextStep();
        }

        public async Task Step2(DialogContext context, CancellationToken cancellationToken)
        {
            await _messagesToSignService.AddAsync(new MessageToSign(context.Update.Message.Chat.Id, context.Update.Message.MessageId));

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Сообщение добавлено для подписи");

            context.EndDialog();
        }
    }
}
