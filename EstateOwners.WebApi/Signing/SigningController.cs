using AutoMapper;
using EstateOwners.App.Signing;
using EstateOwners.App.Telegram.Voting;
using EstateOwners.Domain.Telegram.Voting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slid.Auth.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstateOwners.WebApi.Controllers
{
    [Authorize(Policy = Policy.MustBeAllAccessMode)]
    [Route("api/v1/[controller]")]
	[ApiController]
	public class SigningController : ControllerBase
	{
		private readonly IVoteTelegramMessagesService _service;
        private readonly IMapper _mapper;

        public SigningController(IVoteTelegramMessagesService service, IMapper mapper)
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

        [Authorize(Policy = Policy.MustBeAllOrExportAccessMode)]
        [HttpPost("signature")]
		public async Task<ActionResult> AddUserSignature(IFormFile formData)
        {
            //if (files != null)
            //{
            //    if (files.Length > 0)
            //    {
            //        //Getting FileName
            //        var fileName = Path.GetFileName(files.FileName);
            //        //Getting file Extension
            //        var fileExtension = Path.GetExtension(fileName);
            //        // concatenating  FileName + FileExtension
            //        var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

            //        var objfiles = new Files()
            //        {
            //            DocumentId = 0,
            //            Name = newFileName,
            //            FileType = fileExtension,
            //            CreatedOn = DateTime.Now
            //        };

            //        using (var target = new MemoryStream())
            //        {
            //            files.CopyTo(target);
            //            objfiles.DataFiles = target.ToArray();
            //        }

            //        _context.Files.Add(objfiles);
            //        _context.SaveChanges();

            //    }
            //}

            return Ok();
        }
	}
}