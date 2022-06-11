using EstateOwners.Domain.Telegram.Support;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App.Telegram.Support
{
    public interface IIssueTelegramMessagesService
    {
        Task<IssueTelegramMessage> AddAsync(IssueTelegramMessage message);
        Task<List<IssueTelegramMessage>> GetListAsync(string userId, CancellationToken cancellationToken = default);
    }
}