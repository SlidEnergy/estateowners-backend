namespace EstateOwners.Domain.Telegram
{
    public class UserSignature
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public byte[] Signature { get; set; }

        public string Base64Image { get; set; }

        public UserSignature()
        {

        }

        public UserSignature(string userId, byte[] signature)
        {
            UserId = userId;
            Signature = signature;
        }

        public UserSignature(string userId, string signature)
        {
            UserId = userId;
            Base64Image = signature;
        }
    }
}
