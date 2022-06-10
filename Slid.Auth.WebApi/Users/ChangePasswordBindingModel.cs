using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Slid.Auth.WebApi
{
    public class ChangePasswordBindingModel
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
