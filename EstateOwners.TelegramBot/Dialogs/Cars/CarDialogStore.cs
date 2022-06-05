using EstateOwners.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace EstateOwners.TelegramBot.Dialogs
{
    public class CarDialogStore : AuthDialogStore
    {
        public string Title { get; set; }
        public string Number { get; set; }
        public string Color { get; set; }

        public CarDialogStore()
        {

        }

        public CarDialogStore(ApplicationUser user) :base(user)
        {

        }
    }
}
