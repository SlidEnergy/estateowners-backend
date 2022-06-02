using EstateOwners.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App.Polls
{
    public interface IPollsService
    {
        Task<Poll> AddAsync(Poll poll);
        Task<List<Poll>> GetListAsync(CancellationToken cancellationToken = default);
    }
}