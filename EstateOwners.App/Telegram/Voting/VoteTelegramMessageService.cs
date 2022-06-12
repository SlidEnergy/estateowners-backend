using EstateOwners.Domain.Telegram.Voting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App.Telegram.Voting
{
    public class VoteTelegramMessagesService : IVoteTelegramMessagesService
    {
        private IApplicationDbContext _context;

        public VoteTelegramMessagesService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VoteTelegramMessage> AddAsync(VoteTelegramMessage message)
        {
            _context.VoteTelegramMessages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<List<VoteTelegramMessage>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.VoteTelegramMessages.ToListAsync();
        }

        public async Task VoteAsync(string userId, int messageId)
        {
            var userSignature = new TelegramMessageVote(userId, messageId);

            _context.UserMessageVotes.Add(userSignature);

            await _context.SaveChangesAsync();
        }

        public async Task<VotingStatistic> GetUserMessageVoteCountAsync(int messageId)
        {
            var query = await _context.UserMessageVotes
                .Where(x => x.VoteTelegramMessageId == messageId)
                .Join(_context.Users, v => v.UserId, u => u.Id, (v, u) => u)
                .Join(_context.TrusteeEstates, u => u.TrusteeId, t => t.TrusteeId, (u, t) => new { User = u, Estate = t.Estate })
                .ToListAsync();

            var statistic = new VotingStatistic()
            {
                UserCount = query.Select(x => x.User).Distinct().Count(),
                EstateCount = query.Select(x => x.Estate).Count(),
                TotalArea = query.Sum(x => x.Estate.Area)
            };

            return statistic;
        }
    }
}
