using EstateOwners.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public interface IEstatesService
    {
        Task<Estate> AddEstateAsync(string userId, Estate estate);

        Task<List<Estate>> GetListWithAccessCheckAsync(string userId, string filterByUserId = null,
            CancellationToken cancellationToken = default);


        Task<Estate> GetByIdWithAccessCheckAsync(string userId, int id, CancellationToken cancellationToken = default);
    }
}