using EstateOwners.Domain.Telegram;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App.Telegram.Documents
{
    public class DocumentTelegramMessagesService : IDocumentTelegramMessagesService
    {
        private IApplicationDbContext _context;

        public DocumentTelegramMessagesService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DocumentTelegramMessage> AddAsync(DocumentTelegramMessage message)
        {
            _context.DocumentTelegramMessages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<List<DocumentTelegramMessage>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.DocumentTelegramMessages.ToListAsync();
        }
    }
}
