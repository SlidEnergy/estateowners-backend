namespace EstateOwners.Domain.Signing
{
    public class UserMessageSignature
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public int Messageid { get; set; }

        public UserMessageSignature(string userId, int messageid)
        {
            UserId = userId;
            Messageid = messageid;
        }
    }
}
