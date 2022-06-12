using EstateOwners.Domain;
using EstateOwners.Domain.Telegram.Voting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App.Telegram.Voting
{
    public interface IVoteTelegramMessagesService
    {
        Task<VoteTelegramMessage> AddAsync(VoteTelegramMessage message);
        Task<List<VoteTelegramMessage>> GetListAsync(CancellationToken cancellationToken = default);

        Task VoteAsync(string userId, int messageId);

        Task<VotingStatistic> GetUserMessageVoteCountAsync(int messageId);
    }
}