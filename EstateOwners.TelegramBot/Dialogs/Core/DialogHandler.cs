using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot.Dialogs.Core
{
    internal class DialogHandler : UpdateHandlerBase
    {
        private readonly IDialogManager _dialogManager;

        public DialogHandler(IDialogManager dialogManager)
        {
            _dialogManager = dialogManager;
        }

        public override bool CanHandle(IUpdateContext context)
        {
            return context.IsMessageUpdate() || context.IsCallbackQueryUpdate();
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            var chatId = context.GetChatId().Value;

            var state = _dialogManager.GetUserState(chatId);

            if (state == null)
            {
                await next(context);
                return;
            }

            await _dialogManager.RunDialogAsyncInternal(state.ActiveDialog, state, context);

            await next(context);
        }
    }

    public class DialogState
    {
        public Type ActiveDialog { get; set; }

        public int Step { get; set; } = 0;

        public object Store { get; set; }

        public DialogState(Type activeDialog)
        {
            ActiveDialog = activeDialog;

            var genericType = activeDialog.BaseType.GetGenericArguments()[0];
            Store = Activator.CreateInstance(genericType);
        }
    }

    public class DialogManager : IDialogManager
    {
        private Dictionary<long, DialogState> _states = new Dictionary<long, DialogState>();

        public DialogState SetActiveDialog<TDialog, TStore>(long userId, TStore store = null) where TDialog : DialogBase<TStore> where TStore : class
        {
            return SetActiveDialog(userId, typeof(TDialog), store);
        }

        public DialogState SetActiveDialog<TStore>(long userId, Type dialog, TStore store = null) where TStore : class
        {
            if (userId <= 0)
                throw new ArgumentException("User is not defined", nameof(userId));

            if (!typeof(DialogBase<TStore>).IsAssignableFrom(dialog))
                throw new ArgumentException("Dialog type must be child of DialogBase", nameof(dialog));

            var state = new DialogState(dialog);

            if (store != null)
                state.Store = store;

            _states[userId] = state;

            return state;
        }

        public void ClearActiveDialog(long userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User is not defined", nameof(userId));

            _states.Remove(userId);
        }

        public DialogState GetUserState(long userId)
        {
            if (_states.ContainsKey(userId))
                return _states[userId];

            return null;
        }

        public async Task RunDialogAsyncInternal(Type dialog, DialogState state, IUpdateContext context)
        {
            var genericType = dialog.BaseType.GetGenericArguments()[0];

            var method = typeof(DialogManager).GetMethod(nameof(RunDialogAsync), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            var genericMethod = method.MakeGenericMethod(genericType);

            await (Task)genericMethod.Invoke(this, new object[] { dialog, state, context });
        }

        public async Task RunDialogAsync<TStore>(Type dialog, DialogState state, IUpdateContext context) where TStore : class
        {
            var instance = (DialogBase<TStore>)context.Services.GetService(dialog);

            var dialogContext = new DialogContext<TStore>(instance, this, context, state);

            await instance.HandleAsync(dialogContext);
        }
    }

    public class DialogContext<TStore> : UpdateContext where TStore : class
    {
        private readonly DialogBase<TStore> _dialog;
        private readonly IDialogManager _dialogManager;
        internal readonly DialogState State;

        internal int Step => State.Step;

        public TStore Store => (TStore)State.Store;

        public long ChatId => Update.Message?.Chat.Id ?? Update.CallbackQuery.Message.Chat.Id;

        internal DialogContext(DialogBase<TStore> dialog, IDialogManager dialogManager, IUpdateContext context, DialogState state) : base(context.Bot, context.Update, context.Services)
        {
            _dialog = dialog;
            _dialogManager = dialogManager;
            State = state;
        }

        public async Task ReplaceDialogAsync<TStore>(Type dialog, TStore store = null) where TStore : class
        {
            var state = _dialogManager.SetActiveDialog(ChatId, dialog, store);

            await _dialogManager.RunDialogAsyncInternal(dialog, state, this);
        }

        public async Task ReplaceDialogAsync<TDialog, TStore>(TStore store = null) where TDialog : DialogBase<TStore> where TStore : class
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
