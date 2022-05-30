using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EstateOwners.Domain
{
    public class ApplicationUser : IdentityUser, IUniqueObject<string>
    {
        public int TrusteeId { get; set; }
        [Required]
        public virtual Trustee Trustee { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public ApplicationUser()
        {

        }

        public ApplicationUser(Trustee trustee, string email) : this(trustee.Id, email)
        {
            Trustee = trustee;
        }

        public ApplicationUser(int trusteeId, string email)
        {
            TrusteeId = trusteeId;
            Email = UserName = email;
        }
    }
}
