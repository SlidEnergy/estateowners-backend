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

            var dialog = (DialogBase)context.Services.GetService(state.ActiveDialog);

            var dialogContext = new DialogContext(dialog, _dialogManager, context, state);

            await dialog.HandleAsync(dialogContext);

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

        public void SetActiveDialog<T>(long userId) where T : DialogBase
        {
            SetActiveDialog(userId, typeof(T));
        }

        public void SetActiveDialog(long userId, Type dialog)
        {
            if (userId <= 0)
                throw new ArgumentException("User is not defined", nameof(userId));

            if (!typeof(DialogBase).IsAssignableFrom(dialog))
                throw new ArgumentException("Dialog type must be child of DialogBase", nameof(dialog));

            _states[userId] = new DialogState(dialog);
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

        public void SetUserState(long userId, DialogState state)
        {
            _states[userId] = state;
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

        public void ReplaceDialog(Type dialog)
        {
            _dialogManager.SetActiveDialog(ChatId, dialog);
        }

        public void ReplaceDialog<T>() where T : DialogBase
        {
            _dialogManager.SetActiveDialog(ChatId, typeof(T));
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
