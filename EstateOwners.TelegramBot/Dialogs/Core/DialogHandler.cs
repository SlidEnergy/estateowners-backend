using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;

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

            await _dialogManager.RunDialogAsync(state.ActiveDialog, state, context);

            await next(context);
        }
    }

    public class DialogState
    {
        public Type ActiveDialog { get; set; }

        public int Step { get; set; } = 0;

        public Dictionary<string, object> Values { get; set; } = new Dictionary<string, object>();

        public DialogState(Type activeDialog)
        {
            ActiveDialog = activeDialog;
        }
    }

    public class DialogManager : IDialogManager
    {
        private Dictionary<long, DialogState> _states = new Dictionary<long, DialogState>();

        public DialogState SetActiveDialog<T>(long userId) where T : DialogBase
        {
            return SetActiveDialog(userId, typeof(T));
        }

        public DialogState SetActiveDialog(long userId, Type dialog)
        {
            if (userId <= 0)
                throw new ArgumentException("User is not defined", nameof(userId));

            if (!typeof(DialogBase).IsAssignableFrom(dialog))
                throw new ArgumentException("Dialog type must be child of DialogBase", nameof(dialog));

            var state = new DialogState(dialog);

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

        public async Task RunDialogAsync(Type dialog, DialogState state, IUpdateContext context)
        {
            var instance = (DialogBase)context.Services.GetService(dialog);

            var dialogContext = new DialogContext(instance, this, context, state);

            await instance.HandleAsync(dialogContext);
        }
    }

    public class DialogContext : UpdateContext
    {
        private readonly DialogBase _dialog;
        private readonly IDialogManager _dialogManager;
        internal readonly DialogState State;

        internal int Step => State.Step;

        public Dictionary<string, object> Values => State.Values;

        public long ChatId => Update.Message?.Chat.Id ?? Update.CallbackQuery.Message.Chat.Id;

        internal DialogContext(DialogBase dialog, IDialogManager dialogManager, IUpdateContext context, DialogState state) : base(context.Bot, context.Update, context.Services)
        {
            _dialog = dialog;
            _dialogManager = dialogManager;
            State = state;
        }

        public async Task ReplaceDialogAsync(Type dialog)
        {
            var state = _dialogManager.SetActiveDialog(ChatId, dialog);

            await _dialogManager.RunDialogAsync(dialog, state, this);
        }

        public async Task ReplaceDialogAsync<T>() where T : DialogBase
        {
            await ReplaceDialogAsync(typeof(T));
        }

        public void EndDialog()
        {
            _dialogManager.ClearActiveDialog(ChatId);
        }

        public void NextStep()
        {
            State.Step++;
        }

        public async Task ExecuteStep(DialogStep dialogStep, CancellationToken cancellationToken)
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
