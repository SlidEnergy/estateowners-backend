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
    public class EstatesController : ControllerBase
	{
		private readonly IMapper _mapper;
        private readonly IEstatesService _estatesService;

        public EstatesController(IMapper mapper, IEstatesService estatesService)
		{
			_mapper = mapper;
            _estatesService = estatesService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Dto.Estate>>> GetList(string? userId)
        {
            var currentUserId = User.GetUserId();

            var list = await _estatesService.GetListWithAccessCheckAsync(currentUserId, userId);

            return _mapper.Map<Dto.Estate[]>(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<ActionResult<Dto.Estate>> GetById(int id)
        {
            var userId = User.GetUserId();

            var model = await _estatesService.GetByIdWithAccessCheckAsync(userId, id);

            return _mapper.Map<Dto.Estate>(model);
        }
    }
}
