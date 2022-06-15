using System.ComponentModel.DataAnnotations;

namespace EstateOwners.WebApi.Telegram.Connect
{
    public class UserSignatureBindingModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Salt { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Payload { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Base64Image { get; set; }
    }
}
