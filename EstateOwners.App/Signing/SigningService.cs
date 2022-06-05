using EstateOwners.Domain;
using EstateOwners.Domain.Signing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App.Signing
{
    public class SigningService : ISigningService
    {
        private IApplicationDbContext _context;

        public SigningService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MessageToSign> AddAsync(MessageToSign message)
        {
            _context.MessagesToSign.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<List<MessageToSign>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.MessagesToSign.ToListAsync();
        }

        public async Task SignAsync(string userId, int messageId)
        {
            var userSignature = new UserSignature(userId, messageId);

            _context.UserSignatures.Add(userSignature);

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUserSignatureCountAsync(int messageId)
        {
            return await _context.UserSignatures.Where(x => x.Messageid == messageId).CountAsync();
        }

        public async Task<List<ApplicationUser>> GetUserListWhoLeftSignatureAsync(int messageId)
        {
            var users = await _context.UserSignatures
                .Where(x => x.Messageid == messageId)
                .Join(_context.Users, t => t.UserId, u => u.Id, (t, u) => u)
                .ToListAsync();

            return users;
        }
    }
}
