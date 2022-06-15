using EstateOwners.Domain.Telegram;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EstateOwners.App.Telegram
{
    public class UserSignaturesService : IUserSignaturesService
    {
        private IApplicationDbContext _context;

        public UserSignaturesService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserSignature> AddAsync(UserSignature userSignature)
        {
            _context.UserSignatures.Add(userSignature);
            await _context.SaveChangesAsync();

            return userSignature;
        }

        public async Task<UserSignature> UpdateAsync(UserSignature userSignature)
        {
            _context.Entry(userSignature).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return userSignature;
        }

        public async Task<UserSignature> GetByUser(string userId)
        {
            return await _context.UserSignatures.FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
