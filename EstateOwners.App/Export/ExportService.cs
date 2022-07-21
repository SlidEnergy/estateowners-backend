using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lers.Utils;

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
            var signers = await _context.UserMessageVotes
                .Where(x => x.VoteTelegramMessageId == messageId)
                .Join(_context.Users, t => t.UserId, u => u.Id, (t, u) => u)
                .Join(_context.TrusteeEstates, u => u.TrusteeId, t => t.TrusteeId, (u, t) => new { User = u, Estate = t.Estate, Building = t.Estate.Building })
                .GroupJoin(_context.UserSignatures, x => x.User.Id, s => s.UserId, (x, s) => new { User = x.User, Estate = x.Estate, Building = x.Building, Signature = s})
                .SelectMany(x => x.Signature.DefaultIfEmpty(), (x, s) => new Signer
                {
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    MiddleName = x.User.MiddleName,
                    Type = x.Estate.Type.GetDescription(),
                    Building = x.Building.ShortAddress,
                    Number = x.Estate.Number,
                    Area = x.Estate.Area,
                    Base64Signature = s == null ? null : s.Base64Image
                })
                .ToListAsync();

            return signers;
        }

        public async Task<List<Signer>> GetUsersWithEstatesAsync()
        {
            var signers = await _context.Users
                .Join(_context.TrusteeEstates, u => u.TrusteeId, t => t.TrusteeId, (u, t) => new { User = u, Estate = t.Estate, Building = t.Estate.Building })
                .Select(x => new Signer()
                {
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    MiddleName = x.User.MiddleName,
                    Type = x.Estate.Type.GetDescription(),
                    Building = x.Building.ShortAddress,
                    Number = x.Estate.Number,
                    Area = x.Estate.Area,
                })
                .ToListAsync();

            return signers;
        }
    }
}
