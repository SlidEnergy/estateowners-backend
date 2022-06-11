namespace EstateOwners.Domain.Telegram.Support
{
    public class IssueTelegramMessage
    {
        public int Id { get; set; }

        public long FromChatId { get; set; }

        public int MessageId { get; set; }

        public string UserId { get; set; }

        public IssueTelegramMessage()
        {

        }

        public IssueTelegramMessage(string userId, long fromChatId, int messageId)
        {
            UserId = userId;
            FromChatId = fromChatId;
            MessageId = messageId;
        }
    }
}
