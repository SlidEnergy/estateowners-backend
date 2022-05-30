using EstateOwners.Domain;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public interface IEstatesService
    {
        Task<Estate> AddEstate(string userId, int buildingId, EstateType type, string number);
    }
}