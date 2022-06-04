using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot.Dialogs.Core
{
    public delegate Task DialogStep<TStore>(DialogContext<TStore> context, CancellationToken cancellationToken) where TStore : class;

    public abstract class DialogBase : DialogBase<DialogStore>
    {
    }

    public abstract class DialogBase<TStore> where TStore : class
    {
        //public bool activate { get; set; } = false;

        public virtual bool CanHandle(IUpdateContext context) => true;

        List<DialogStep<TStore>> _steps = new List<DialogStep<TStore>>();

        public void AddStep(DialogStep<TStore> step)
        {
            _steps.Add(step);
        }

        public virtual async Task HandleAsync(DialogContext<TStore> context, CancellationToken cancellationToken = default)
        { 
            //if (!activate)
            //    return;

            await ExecuteStep(context, cancellationToken);
        }

        internal int GetStep(DialogStep<TStore> step)
        {
            return _steps.IndexOf(step);
        }

        private async Task ExecuteStep(DialogContext<TStore> context, CancellationToken cancellationToken = default)
        {
            await _steps[context.Step](context, cancellationToken);
        }
    }
}
