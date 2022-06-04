using System;
using System.Collections.Generic;
using System.Text;

namespace EstateOwners.TelegramBot.Dialogs
{
    public class NewUserDialogStore
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
