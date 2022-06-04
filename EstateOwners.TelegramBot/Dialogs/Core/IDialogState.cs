using System;

namespace EstateOwners.TelegramBot.Dialogs.Core
{
    public interface IDialogState
    {
        Type ActiveDialog { get; set; }
        int Step { get; set; }
        object Store { get; set; }
    }
}