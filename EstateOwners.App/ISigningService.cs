using EstateOwners.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public interface ISigningService
    {
        Task<MessageToSign> AddAsync(MessageToSign message);
        Task<List<MessageToSign>> GetListAsync(CancellationToken cancellationToken = default);

        Task SignAsync(string userId, int messageId);

        Task<int> GetUserSignatureCountAsync(int messageId);
    }
}