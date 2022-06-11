namespace EstateOwners.Domain.Telegram.Voting
{
    public class TelegramMessageVote
    {
        public string UserId { get; set; }

        public int VoteTelegramMessageId { get; set; }

        public TelegramMessageVote()
        {

        }

        public TelegramMessageVote(string userId, int messageid)
        {
            UserId = userId;
            VoteTelegramMessageId = messageid;
        }
    }
}
