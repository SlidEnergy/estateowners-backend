using EstateOwners.Domain;
using EstateOwners.Domain.Candidates;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App.Signing
{
    public class CandidatesService : ICandidatesService
    {
        private IApplicationDbContext _context;

        public CandidatesService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Candidate>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Candidates.ToListAsync();
        }

        public async Task VoteForCandidateAsync(string userId, int candidateId)
        {
            var vote = new VoteForCandidate(userId, candidateId);

            _context.VotesForCandidates.Add(vote);

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetVotesForCandidatesCount(int candidateId)
        {
            return await _context.VotesForCandidates.CountAsync(x => x.CandidateId == candidateId);
        }

        public async Task AddAsync(string userId, CandidateType type)
        {
            var candidate = new Candidate(userId, type);

            _context.Candidates.Add(candidate);

            await _context.SaveChangesAsync();
        }
    }
}
