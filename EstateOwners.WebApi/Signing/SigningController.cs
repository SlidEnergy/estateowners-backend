using AutoMapper;
using EstateOwners.App;
using EstateOwners.App.Signing;
using EstateOwners.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstateOwners.WebApi.Controllers
{
    [Authorize(Policy = Policy.MustBeAllOrExportAccessMode)]
	[Route("api/v1/[controller]")]
	[ApiController]
	public class SigningController : ControllerBase
	{
		private readonly ISigningService _service;
        private readonly IMapper _mapper;

        public SigningController(ISigningService service, IMapper mapper)
		{
			_service = service;
            _mapper = mapper;
        }

		[HttpGet("messages")]
		public async Task<ActionResult<List<MessageToSign>>> GetList()
		{
			var userId = User.GetUserId();

			return await _service.GetListAsync();
		}

		[HttpGet("messages/{messageId}/users")]
		public async Task<ActionResult<List<Dto.User>>> GetUsers(int messageId)
		{
			var userId = User.GetUserId();

			var users =  await _service.GetUserListWhoLeftSignatureAsync(messageId);

			return _mapper.Map<List<Dto.User>>(users);
		}
	}
}