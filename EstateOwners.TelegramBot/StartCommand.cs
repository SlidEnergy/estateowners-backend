using EstateOwners.App;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace EstateOwners.TelegramBot
{
    public class StartCommandArgs : ICommandArgs
    {
        public string RawInput { get; set; }
        public string ArgsInput { get; set; }
    }
    public class StartCommand : CommandBase<StartCommandArgs>
    {
        private readonly IUsersService _usersService;

        public StartCommand(IUsersService usersService) : base(name: "start")
        {
            _usersService = usersService;
        }

        public override async Task<UpdateHandlingResult> HandleCommand(Update update, StartCommandArgs args)
        {
            MainDialog.activate = false;
            NewEstateDialog.activate = false;
            NewUserDialog.activate = false;

            var chatId = update.Message.Chat.Id;

            await Bot.Client.SendTextMessageAsync(
                chatId,
                "Приветствую. Описание бота.");

            var user = await _usersService.GetByAuthTokenAsync(chatId.ToString(), Domain.AuthTokenType.TelegramChatId);

            if (user == null)
            {
                NewUserDialog.activate = true;
                return UpdateHandlingResult.Continue;
            }
            else
            {
                MainDialog.activate = true;
                return UpdateHandlingResult.Continue;
            }
        }
    }
}
