using EstateOwners.Domain;
using EstateOwners.Domain.Signing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App.Polls
{
    public class PollsService : IPollsService
    {
        private IApplicationDbContext _context;

        public PollsService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Poll> AddAsync(Poll poll)
        {
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            return poll;
        }

        public async Task<List<Poll>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Polls.ToListAsync();
        }
    }
}
