using EstateOwners.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Slid.Auth.Core;

namespace Slid.Auth.WebApi
{
    [Route("api/v1/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
        private readonly IUsersService _usersService;

        public AuthController(IUsersService usersService)
		{
			_usersService = usersService;
        }

		[HttpPost("changepassword")]
		[ProducesResponseType(200)]
        [Authorize]
		public async Task<ActionResult> ChangePassword(ChangePasswordBindingModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var userId = User.GetUserId();

			var result = await _usersService.ChangePassword(userId, model.CurrentPassword, model.NewPassword);

			if (!result.Succeeded)
			{
				foreach (var e in result.Errors)
				{
					ModelState.TryAddModelError(e.Code, e.Description);
				}

				return BadRequest(ModelState);
			}

			return Ok();
		}
	}
}
