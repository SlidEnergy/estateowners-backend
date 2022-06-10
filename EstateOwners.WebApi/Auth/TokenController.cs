using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using EstateOwners.App;
using EstateOwners.WebApi.Dto;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace EstateOwners.WebApi
{
	[Route("api/v1/[controller]")]
    [ApiController]
    public sealed class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost()]
        [ProducesResponseType(200)]
        public async Task<ActionResult<TokenInfo>> GetToken(LoginBindingModel userData)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var tokens = await _tokenService.CheckCredentialsAndGetToken(userData.Email, userData.Password);

                return new TokenInfo() { Token = tokens.Token, RefreshToken = tokens.RefreshToken, Email = userData.Email };
            }
            catch (AuthenticationException)
            {
                return BadRequest();
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenInfo>> Refresh(TokensCortage tokens)
        {
            try
            {
                var newTokens = await _tokenService.RefreshToken(tokens.Token, tokens.RefreshToken);

				return new TokenInfo() { Token = newTokens.Token, RefreshToken = newTokens.RefreshToken };
            }
            catch (SecurityTokenException)
            {
                return BadRequest();
            }
        }
    }
}
