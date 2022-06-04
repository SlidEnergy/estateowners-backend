using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using EstateOwners.Domain;

namespace EstateOwners.App
{
	public interface IUsersService
	{
		Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);

		Task<IdentityResult> CreateUserAsync(ApplicationUser user, string token, AuthTokenType tokenType);

		Task<ApplicationUser> GetByIdAsync(string userId);

		Task<bool> IsAdminAsync(ApplicationUser user);

		Task<List<ApplicationUser>> GetListAsyncAsync();

		Task<ApplicationUser> GetByAuthTokenAsync(string token, AuthTokenType type);
	}
}