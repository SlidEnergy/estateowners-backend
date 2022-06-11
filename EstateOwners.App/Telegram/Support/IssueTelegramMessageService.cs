using EstateOwners.Domain.Telegram;
using EstateOwners.Domain.Telegram.Support;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App.Telegram.Support
{
    public class IssueTelegramMessagesService : IIssueTelegramMessagesService
    {
        private IApplicationDbContext _context;

        public IssueTelegramMessagesService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IssueTelegramMessage> AddAsync(IssueTelegramMessage message)
        {
            _context.IssueTelegramMessages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<List<IssueTelegramMessage>> GetListAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.IssueTelegramMessages.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
