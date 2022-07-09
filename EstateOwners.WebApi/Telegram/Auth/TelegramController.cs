using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Slid.Auth.Core;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using EstateOwners.WebApi.Dto;

namespace EstateOwners.WebApi.Telegram.Connect
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

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenInfo>> GetToken(TelegramUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var tokenInfo = await _service.GetTokenAsync(user);

                return tokenInfo;
            }
            catch (AuthenticationException)
            {
                return BadRequest();
            }
        }
    }
}