using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public interface IExportService
    {
        Task<List<Signer>> GetSignersAsync(int messageId);
        Task<List<UserWithEstate>> GetUsersWithEstatesAsync();
    }
}