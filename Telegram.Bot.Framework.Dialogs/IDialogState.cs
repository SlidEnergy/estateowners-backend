using System;

namespace Telegram.Bot.Framework.Dialogs
{
    public interface IDialogState
    {
        Type ActiveDialog { get; set; }
        int Step { get; set; }
        object Store { get; set; }
    }
}