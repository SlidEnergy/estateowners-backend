using AspNetCore.Authentication.ApiKey;
using EstateOwners.App;
using EstateOwners.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EstateOwners.WebApi.Auth
{
    public class ApiKeyProvider : IApiKeyProvider
	{
		private readonly ILogger<ApiKeyProvider> _logger;
		private readonly IUsersService _usersService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IClaimsGenerator _claimsGenerator;

		public ApiKeyProvider(ILogger<ApiKeyProvider> logger, IUsersService usersService, UserManager<ApplicationUser> userManager, IClaimsGenerator claimsGenerator)
		{
			_logger = logger;
			_usersService = usersService;
			_userManager = userManager;
			_claimsGenerator = claimsGenerator;
		}

		public async Task<IApiKey> ProvideAsync(string key)
		{
			try
			{
				if (string.IsNullOrEmpty(key))
					return null;

				ApplicationUser user = await _usersService.GetByAuthTokenAsync(key, AuthTokenType.ApiKey);

				if (user == null)
					return null;

				var roles = await _userManager.GetRolesAsync(user);

				var claims = _claimsGenerator.CreateClaims(user, roles, AccessMode.Export);

				var apiKey = new ApiKey(key, user.Id, claims);

				return apiKey;
			}
			catch (System.Exception exception)
			{
				_logger.LogError(exception, exception.Message);
				throw;
			}
		}
	}
}
