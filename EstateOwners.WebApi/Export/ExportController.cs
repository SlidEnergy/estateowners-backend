using AutoMapper;
using EstateOwners.App;
using EstateOwners.App.Signing;
using EstateOwners.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
	}
}