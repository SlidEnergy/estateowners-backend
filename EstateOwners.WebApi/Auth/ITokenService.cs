using System.Threading.Tasks;
using EstateOwners.App;
using EstateOwners.Domain;

namespace EstateOwners.WebApi
{
	public interface ITokenService
	{
		Task<TokensCortage> GenerateAccessAndRefreshTokens(ApplicationUser user);
		Task<string> GenerateToken(ApplicationUser user, AuthTokenType type);
		Task<TokensCortage> RefreshToken(string token, string refreshToken);
		Task<TokensCortage> CheckCredentialsAndGetToken(string email, string password);
	}
}