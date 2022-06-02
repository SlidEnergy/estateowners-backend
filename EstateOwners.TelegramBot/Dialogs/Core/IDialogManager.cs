using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot.Dialogs.Core
{
    public interface IDialogManager
    {
        DialogState GetUserState(long userId);

        DialogState SetActiveDialog(long userId, Type dialog);

        DialogState SetActiveDialog<T>(long userId) where T : DialogBase;

        void ClearActiveDialog(long userId);

        Task RunDialogAsync(Type dialog, DialogState state, IUpdateContext context);
    }
}