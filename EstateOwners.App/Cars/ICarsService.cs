using EstateOwners.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public interface ICarsService
    {
        Task<Car> AddAsync(string userId, Car car);
        Task<List<Car>> GetListWithAccessCheckAsync(string userId, CancellationToken cancellationToken = default);
    }
}