using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework.Dialogs
{
    public class DialogManager : IDialogManager
    {
        private readonly Dictionary<long, DialogState> _states = new Dictionary<long, DialogState>();

        public DialogState SetActiveDialog<TDialog, TStore>(long userId, TStore store = null) where TDialog : Dialog<TStore> where TStore : class
        {
            return SetActiveDialog(userId, typeof(TDialog), store);
        }

        public DialogState SetActiveDialog<TStore>(long userId, Type dialog, TStore store = null) where TStore : class
        {
            if (userId <= 0)
                throw new ArgumentException("User is not defined", nameof(userId));

            if (!typeof(Dialog<TStore>).IsAssignableFrom(dialog))
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
            var instance = (Dialog<TStore>)context.Services.GetService(dialog);

            var dialogContext = new DialogContext<TStore>(instance, this, context, state);

            await instance.HandleAsync(dialogContext);
        }
    }
}