using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework.Dialogs
{
    public delegate Task DialogStep<TStore>(DialogContext<TStore> context, CancellationToken cancellationToken) where TStore : class;

    public abstract class Dialog<TStore> where TStore : class
    {
        public virtual bool CanHandle(IUpdateContext context) => true;

        List<DialogStep<TStore>> _steps = new List<DialogStep<TStore>>();

        public void AddStep(DialogStep<TStore> step)
        {
            _steps.Add(step);
        }

        public virtual async Task HandleAsync(DialogContext<TStore> context, CancellationToken cancellationToken = default)
        {
            await ExecuteStep(context, cancellationToken);
        }

        internal int GetStep(DialogStep<TStore> step)
        {
            return _steps.IndexOf(step);
        }

        private async Task ExecuteStep(DialogContext<TStore> context, CancellationToken cancellationToken = default)
        {
            var stepDelegate = _steps[context.Step];

            var attribute = stepDelegate.Method.GetCustomAttributes(typeof(EndDialogStepFilterAttribute), true).FirstOrDefault();

            if (attribute != null)
            {
                var endDialogStepFilter = (EndDialogStepFilterAttribute)attribute;

                if (!endDialogStepFilter.Type.HasFlag(context.Update.Type))
                {
                    await context.Bot.Client.SendTextMessageAsync(context.ChatId, endDialogStepFilter.Message);
                    context.EndDialog();
                    return;
                }
            }

            await stepDelegate(context, cancellationToken);
        }
    }
}
