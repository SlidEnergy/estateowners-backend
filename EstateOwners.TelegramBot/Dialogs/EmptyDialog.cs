using EstateOwners.TelegramBot.Dialogs.Core;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.TelegramBot
{
    internal class EmptyDialog : DialogBase
    {
        public EmptyDialog()
        {
            AddStep(Step1);
        }

        public async Task Step1(DialogContext context, CancellationToken cancellationToken)
        {
            context.EndDialog();
        }
    }
}
