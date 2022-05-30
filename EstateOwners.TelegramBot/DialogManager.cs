using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Framework.Abstractions;

namespace EstateOwners.TelegramBot
{
    public class DialogManager
    {
        IUpdateHandler dialog;

        public IUpdateHandler CreateNewUserDialog()
        {
            dialog = new NewUserDialog();

            return dialog;
        }

    }
}
