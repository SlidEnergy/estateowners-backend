using EstateOwners.Domain;
using EstateOwners.Domain.Telegram;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App.Telegram.Documents
{
    public interface IDocumentTelegramMessagesService
    {
        Task<DocumentTelegramMessage> AddAsync(DocumentTelegramMessage message);
        Task<List<DocumentTelegramMessage>> GetListAsync(CancellationToken cancellationToken = default);
    }
}