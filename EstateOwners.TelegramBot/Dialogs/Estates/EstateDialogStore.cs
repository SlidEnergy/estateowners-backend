using EstateOwners.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace EstateOwners.TelegramBot.Dialogs
{
    public class EstateDialogStore : AuthDialogStore
    {
        public List<Building> Buildings { get; set; }

        public Building Building { get; set; }

        public string Number { get; set; }

        public EstateType Type { get; set; }
        
        public float Area { get; set; }

        public EstateDialogStore()
        {

        }

        public EstateDialogStore(ApplicationUser user) :base(user)
        {

        }
    }
}
