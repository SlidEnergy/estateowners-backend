using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public class ExportService : IExportService
    {
        private IApplicationDbContext _context;

        public ExportService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Signer>> GetSignersAsync(int messageId)
        {
            var signers = await _context.UserMessageSignatures
                .Where(x => x.Messageid == messageId)
                .Join(_context.Users, t => t.UserId, u => u.Id, (t, u) => u)
                .Join(_context.TrusteeEstates, t => t.TrusteeId, u => u.TrusteeId, (u, t) => new { User = u, Estate = t.Estate, Building = t.Estate.Building })
                .Select(x => new Signer
                {
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    MiddleName = x.User.MiddleName,
                    Type = x.Estate.Type.ToString(),
                    Building = x.Building.ShortAddress,
                    Number = x.Estate.Number,
                    Area = x.Estate.Area
                })
                .ToListAsync();

            return signers;
        }
    }
}
