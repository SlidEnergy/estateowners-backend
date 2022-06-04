namespace EstateOwners.Domain
{
    public class TelegramUser
    {
        public int Id { get; set; }
        public bool IsBot { get; set; }
        public string LanguageCode { get; set; }
        public long TelegramUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public string UserId { get; set; }
    }
}
