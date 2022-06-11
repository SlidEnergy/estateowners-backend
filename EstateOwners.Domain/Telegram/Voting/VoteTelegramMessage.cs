namespace EstateOwners.Domain.Telegram.Voting
{
    public class VoteTelegramMessage
    {
        public int Id { get; set; }
        public long FromChatId { get; set; }

        public int MessageId { get; set; }

        public VoteTelegramMessage()
        {

        }
        
        public VoteTelegramMessage(long fromChatId, int messageId)
        {
            FromChatId = fromChatId;
            MessageId = messageId;
        }
    }
}
