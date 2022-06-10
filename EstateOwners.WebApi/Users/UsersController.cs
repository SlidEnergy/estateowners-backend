using AutoMapper;
using EstateOwners.App;
using EstateOwners.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slid.Auth.Core;

namespace EstateOwners.WebApi
{
    [Route("api/v1/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IUsersService _usersService;

		public UsersController(IMapper mapper, IUsersService usersService)
		{
			_mapper = mapper;
			_usersService = usersService;
		}

		[HttpGet]
		[ProducesResponseType(200)]
		[Authorize(Policy = Policy.MustBeAdmin)]
		public async Task<ActionResult<IEnumerable<Dto.User>>> GetList()
		{
			var users = await _usersService.GetListAsyncAsync();

			return _mapper.Map<Dto.User[]>(users);
		}

		[HttpGet("current")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		[Authorize]
		public async Task<ActionResult<Dto.User>> GetCurrentUser()
		{
			var userId = User.GetUserId();
			
			var user = await _usersService.GetByIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			bool isAdmin = await _usersService.IsAdminAsync(user);

			return _mapper.Map<Dto.User>(user, opt => opt.AfterMap((src, dest) => ((Dto.User)dest).IsAdmin = isAdmin));
		}

		[HttpPost("register")]
		public async Task<ActionResult<Dto.User>> Register(RegisterBindingModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var user = _mapper.Map<ApplicationUser>(model);

			var result = await _usersService.CreateUserAsync(user, model.Password);

			if (!result.Succeeded) {
				foreach (var e in result.Errors)
				{
					ModelState.TryAddModelError(e.Code, e.Description);
				}

				return BadRequest(ModelState);
			}

			return Created("", _mapper.Map<Dto.User>(user));
		}
	}
}
