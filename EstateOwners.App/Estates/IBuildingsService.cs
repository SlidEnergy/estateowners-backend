using EstateOwners.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public interface IBuildingsService
    {
        Task<List<Building>> GetListAsync();
    }
}