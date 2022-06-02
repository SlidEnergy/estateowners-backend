using System;
using System.Collections.Generic;
using System.Text;

namespace EstateOwners.Domain
{
    public class Poll
    {
        public int Id { get; set; }
        public long FromChatId { get; set; }

        public int MessageId { get; set; }

        public string PollId { get; set; }
    
        public Poll(long fromChatId, int messageId, string pollId)
        {
            FromChatId = fromChatId;
            MessageId = messageId;
            PollId = pollId;
        }
    }
}
