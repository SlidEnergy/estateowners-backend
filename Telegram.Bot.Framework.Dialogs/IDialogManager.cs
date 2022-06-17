using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework.Dialogs
{
    public interface IDialogManager
    {
        DialogState GetUserState(long userId);

        DialogState SetActiveDialog<TStore>(long userId, Type dialog, TStore store = null) where TStore : class;

        DialogState SetActiveDialog<TDialog, TStore>(long userId, TStore store = null) where TDialog : Dialog<TStore> where TStore : class;

        void ClearActiveDialog(long userId);

        Task RunDialogAsyncInternal(Type dialog, DialogState state, IUpdateContext context);

        Task RunDialogAsync<TStore>(Type dialog, DialogState state, IUpdateContext context) where TStore : class;
    }
}