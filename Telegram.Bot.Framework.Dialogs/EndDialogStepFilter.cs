using System;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Dialogs
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class EndDialogStepFilterAttribute : Attribute
    {
        public UpdateType Type { get; }
        public string Message { get; }

        public EndDialogStepFilterAttribute(UpdateType type, string message, string[] callbackQueryValues = null)
        {
            Type = type;
            Message = message;
        }
    }
}
