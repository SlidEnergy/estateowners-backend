using EstateOwners.Domain.Telegram;
using System.Threading.Tasks;

namespace EstateOwners.App.Telegram
{
    public interface IUserSignaturesService
    {
        Task<UserSignature> AddAsync(UserSignature userSignature);

        Task<UserSignature> GetByUser(string userId);

        Task<UserSignature> UpdateAsync(UserSignature userSignature)
    }
}