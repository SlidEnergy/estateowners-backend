using System.Threading.Tasks;
using EstateOwners.Domain;

namespace EstateOwners.App
{
	public interface IAuthTokenService
	{
		Task AddToken(string userId, string token, AuthTokenType type);
		Task<AuthToken> FindAnyToken(string token);
		Task<AuthToken> UpdateToken(AuthToken oldToken, string newToken);
	}
}