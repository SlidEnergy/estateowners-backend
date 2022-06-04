using EstateOwners.Domain;

namespace EstateOwners.TelegramBot.Dialogs
{
    public class AuthDialogStore
    {
        public ApplicationUser User { get; set; }

        public AuthDialogStore()
        {

        }
        public AuthDialogStore(ApplicationUser user)
        {
            User = user;
        }
    }
}
