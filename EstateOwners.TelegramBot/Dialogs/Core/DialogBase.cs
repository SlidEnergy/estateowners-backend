using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot.Dialogs.Core
{
    public delegate Task DialogStep(DialogContext context, CancellationToken cancellationToken);

    public abstract class DialogBase
    {
        //public bool activate { get; set; } = false;

        public virtual bool CanHandle(IUpdateContext context) => true;

        List<DialogStep> _steps = new List<DialogStep>();

        public void AddStep(DialogStep step)
        {
            _steps.Add(step);
        }

        public virtual async Task HandleAsync(DialogContext context, CancellationToken cancellationToken = default)
        { 
            //if (!activate)
            //    return;

            await ExecuteStep(context, cancellationToken);
        }

        internal int GetStep(DialogStep step)
        {
            return _steps.IndexOf(step);
        }

        private async Task ExecuteStep(DialogContext context, CancellationToken cancellationToken = default)
        {
            await _steps[context.Step](context, cancellationToken);
        }
    }
}
