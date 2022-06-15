using AutoMapper;
using EstateOwners.App.Telegram.Voting;
using EstateOwners.Domain.Telegram.Voting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Slid.Auth.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstateOwners.WebApi.Telegram
{
    [Authorize(Policy = Policy.MustBeAllAccessMode)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VotingController : ControllerBase
    {
        private readonly IVoteTelegramMessagesService _service;
        private readonly IMapper _mapper;

        public VotingController(IVoteTelegramMessagesService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("messages")]
        public async Task<ActionResult<List<VoteTelegramMessage>>> GetList()
        {
            var userId = User.GetUserId();

            return await _service.GetListAsync();
        }
    }
}