using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework.Dialogs
{
    public class DialogContext<TStore> : UpdateContext where TStore : class
    {
        private readonly Dialog<TStore> _dialog;
        private readonly IDialogManager _dialogManager;
        internal readonly DialogState State;

        internal int Step => State.Step;

        public TStore Store => (TStore)State.Store;

        public long ChatId => Update.Message?.Chat.Id ?? Update.CallbackQuery.Message.Chat.Id;

        internal DialogContext(Dialog<TStore> dialog, IDialogManager dialogManager, IUpdateContext context, DialogState state) : base(context.Bot, context.Update, context.Services)
        {
            _dialog = dialog;
            _dialogManager = dialogManager;
            State = state;
        }

        public async Task ReplaceDialogAsync<T>(Type dialog, T store = null) where T : class
        {
            var state = _dialogManager.SetActiveDialog(ChatId, dialog, store);

            await _dialogManager.RunDialogAsyncInternal(dialog, state, this);
        }

        public async Task ReplaceDialogAsync<TDialog, T>(T store = null) where TDialog : Dialog<T> where T : class
        {
            await ReplaceDialogAsync(typeof(TDialog), store);
        }

        public void EndDialog()
        {
            _dialogManager.ClearActiveDialog(ChatId);
        }

        public void NextStep()
        {
            State.Step++;
        }

        public async Task ExecuteStepAsync(DialogStep<TStore> dialogStep, CancellationToken cancellationToken)
        {
            var step = _dialog.GetStep(dialogStep);

            if (step >= 0)
            {
                State.Step = step;
                await _dialog.HandleAsync(this, cancellationToken);
            }
        }
    }
}