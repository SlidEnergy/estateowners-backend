using System;

namespace Telegram.Bot.Framework.Dialogs
{
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
}