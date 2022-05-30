using EstateOwners.Domain;
using System.Threading.Tasks;

namespace EstateOwners.App
{
	public interface IAuthTokensRepository: IRepository<AuthToken, int>
	{
		Task<AuthToken> FindAnyToken(string token);
	}
}
