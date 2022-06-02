using EstateOwners.App;
using EstateOwners.App.Polls;
using EstateOwners.App.Signing;
using EstateOwners.Domain;
using EstateOwners.TelegramBot.Dialogs.Core;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;

namespace EstateOwners.TelegramBot.Dialogs.Polls
{
    internal class AddPollDialog : DialogBase
    {
        private readonly IPollsService _pollsService;
        private readonly IUsersService _usersService;

        public AddPollDialog(IPollsService pollsService, IUsersService usersService)
        {
            _pollsService = pollsService;
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
                    "Добавьте новый опрос через меню сверху справа");

            context.NextStep();
        }

        public async Task Step2(DialogContext context, CancellationToken cancellationToken)
        {
            await _pollsService.AddAsync(new Poll(context.Update.Message.Chat.Id, context.Update.Message.MessageId, context.Update.Message.Poll.Id));

            await context.Bot.Client.SendTextMessageAsync(
                    context.ChatId,
                    "Опрос добавлен");

            context.EndDialog();
        }
    }
}
