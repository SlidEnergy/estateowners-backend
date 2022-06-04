using EstateOwners.App;
using EstateOwners.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace EstateOwners.WebApi
{

    [Route("api/v1/[controller]")]
	[ApiController]
	public class ApiKeysController : ControllerBase
	{
		private readonly ITokenService _tokenService;
		private readonly IUsersService _usersService;

		public ApiKeysController(ITokenService tokenService, IUsersService usersService)
		{
			_tokenService = tokenService;
			_usersService = usersService;
		}

		[Authorize(Policy = Policy.MustBeAllAccessMode)]
		[HttpPost()]
		[ProducesResponseType(200)]
		public async Task<ActionResult<string>> GetApiKey()
		{
			var userId = User.GetUserId();

			var user = await _usersService.GetByIdAsync(userId);

			if (user == null)
				throw new AuthenticationException();

			return await _tokenService.GenerateToken(user, AuthTokenType.ApiKey);
		}
	}
}
