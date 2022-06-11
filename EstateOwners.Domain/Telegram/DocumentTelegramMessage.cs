using EstateOwners.Domain.Telegram;

namespace EstateOwners.Domain.Telegram
{
    public class DocumentTelegramMessage
    {
        public int Id { get; set; }
        public long FromChatId { get; set; }

        public int MessageId { get; set; }

        public DocumentTelegramMessage()
        {

        }

        public DocumentTelegramMessage(long fromChatId, int messageId)
        {
            FromChatId = fromChatId;
            MessageId = messageId;
        }
    }
}
