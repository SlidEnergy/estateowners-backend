using EstateOwners.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App.Signing
{
    public interface ICandidatesService
    {
        Task AddAsync(string userId, CandidateType type);
        Task<List<Candidate>> GetListAsync(CancellationToken cancellationToken = default);
        Task<int> GetVotesForCandidatesCount(int candidateId);
        Task VoteForCandidateAsync(string userId, int candidateId);
    }
}