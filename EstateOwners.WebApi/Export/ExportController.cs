using AutoMapper;
using EstateOwners.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Slid.Auth.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstateOwners.WebApi.Controllers
{
    [Authorize(Policy = Policy.MustBeAllOrExportAccessMode)]
	[Route("api/v1/[controller]")]
	[ApiController]
	public class ExportController : ControllerBase
	{
		private readonly IExportService _service;
        private readonly IMapper _mapper;

        public ExportController(IExportService service, IMapper mapper)
		{
			_service = service;
            _mapper = mapper;
        }


        [HttpGet("messages/{messageId}/signers")]
		public async Task<ActionResult<List<Signer>>> GetSigners(int messageId)
		{
			var userId = User.GetUserId();

			var signers =  await _service.GetSignersAsync(messageId);

			return signers;
		}

        [HttpGet]
        public async Task<ActionResult<List<UserWithEstate>>> GetUsersWithEstates()
        {
            var userId = User.GetUserId();

            var export = await _service.GetUsersWithEstatesAsync();

            return export;
        }
	}
}