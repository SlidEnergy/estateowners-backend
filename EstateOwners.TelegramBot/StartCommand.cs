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
        public static bool authorized = false;

        public StartCommand() : base(name: "start") { }

        public override async Task<UpdateHandlingResult> HandleCommand(Update update, StartCommandArgs args)
        {
            await Bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Приветствую. Описание бота.");

            //var authorized = update.Message.Chat.Id == 123;

            if (!authorized)
            {
                NewUserDialog.activate = true;
                return UpdateHandlingResult.Continue;
            }
            else
            {
                MainDialog.activate = true;
                return UpdateHandlingResult.Continue;
            }

            return UpdateHandlingResult.Handled;
        }
    }
}
