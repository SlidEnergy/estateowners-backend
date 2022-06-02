using System;

namespace EstateOwners.TelegramBot.Dialogs.Core
{
    public interface IDialogManager
    {
        DialogState GetUserState(long userId);

        void SetUserState(long userId, DialogState userState);

        void SetActiveDialog(long userId, Type dialog);

        void SetActiveDialog<T>(long userId) where T : DialogBase;

        void ClearActiveDialog(long userId);
    }
}