namespace EstateOwners.Domain.Signing
{
    public class UserSignature
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public int Messageid { get; set; }

        public UserSignature(string userId, int messageid)
        {
            UserId = userId;
            Messageid = messageid;
        }
    }
}
