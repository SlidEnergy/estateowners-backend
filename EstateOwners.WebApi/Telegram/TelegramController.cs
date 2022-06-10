using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Slid.Auth.Core;
using System;
using System.Threading.Tasks;

namespace EstateOwners.WebApi.Controllers
{
    [Authorize()]
	[Route("api/v1/[controller]")]
	[ApiController]
	public class TelegramController : ControllerBase
	{
		private readonly ITelegramService _service;

		public TelegramController(ITelegramService service)
		{
			_service = service;
		}

		[HttpPost]
		public async Task<ActionResult> Connect(TelegramUser user)
		{
			var userId = User.GetUserId();

			try
			{
				await _service.ConnectTelegramUser(userId, user);
			}
			catch (ArgumentException exc)
			{
				return BadRequest();
			}

			return Ok();
		}
	}
}