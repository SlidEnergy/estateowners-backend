using System;
using System.Collections.Generic;
using System.Text;

namespace EstateOwners.Domain
{
    public class MessageToSign
    {
        public int Id { get; set; }
        public long FromChatId { get; set; }

        public int MessageId { get; set; }
    
        public MessageToSign(long fromChatId, int messageId)
        {
            FromChatId = fromChatId;
            MessageId = messageId;
        }
    }
}
