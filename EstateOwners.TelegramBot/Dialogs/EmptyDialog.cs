using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Dialogs;

namespace EstateOwners.TelegramBot
{
    internal class EmptyDialog : Dialog<DialogStore>
    {
        public EmptyDialog()
        {
            AddStep(Step1);
        }

        public async Task Step1(DialogContext<DialogStore> context, CancellationToken cancellationToken)
        {
            context.EndDialog();
        }
    }
}
