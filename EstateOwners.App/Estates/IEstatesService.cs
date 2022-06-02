using EstateOwners.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public interface IEstatesService
    {
        Task<Estate> AddEstateAsync(string userId, int buildingId, EstateType type, string number);

        Task<List<Estate>> GetListWithAccessCheckAsync(string userId, CancellationToken cancellationToken = default);
    }
}