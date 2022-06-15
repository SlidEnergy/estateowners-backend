using AutoMapper;
using EstateOwners.App;
using EstateOwners.App.Telegram;
using EstateOwners.Domain.Telegram;
using EstateOwners.TelegramBot.Dialogs;
using EstateOwners.WebApi.Telegram.Connect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Slid.Auth.Core;
using Slid.Security;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EstateOwners.WebApi.Telegram
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserSignaturesController : ControllerBase
    {
        private readonly IUserSignaturesService _service;
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;
        private readonly TelegramBotSettings _telegramBotSettings;

        public UserSignaturesController(IUserSignaturesService service, IMapper mapper, IOptions<TelegramBotSettings> options, IUsersService usersService)
        {
            _service = service;
            _mapper = mapper;
            _usersService = usersService;
            _telegramBotSettings = options.Value;
        }

        [HttpPost()]
        public async Task<ActionResult> AddUserSignature(IFormFile formData)
        {
            var userId = User.GetUserId();

            if (formData == null || formData.Length == 0)
                return BadRequest();

            //Getting FileName
            var fileName = Path.GetFileName(formData.FileName);
            //Getting file Extension
            var fileExtension = Path.GetExtension(fileName);
            // concatenating  FileName + FileExtension
            var newFileName = string.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

            using var ms = new MemoryStream();
            formData.CopyTo(ms);

            UserSignature userSignature = new UserSignature(userId, ms.ToArray());

            await _service.AddAsync(userSignature);

            return Ok();
        }

        [HttpPost("json")]
        public async Task<ActionResult> AddUserSignature(UserSignatureBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var key = _telegramBotSettings.Aes256Key.Substring(0, _telegramBotSettings.Aes256Key.Length - model.Salt.Length) + model.Salt;
            var json = Aes256.DecryptString(key, model.Payload);
            var payload = JsonConvert.DeserializeObject<TelegramGamePayload>(json);

            var user = await _usersService.GetByAuthTokenAsync(payload.ChatId.ToString(), Domain.AuthTokenType.TelegramUserId);

            var userId = user.Id;

            //using var ms = new MemoryStream();
            //formData.CopyTo(ms);

            var userSignature = await _service.GetByUser(userId);

            if (userSignature == null)
            {
                userSignature = new UserSignature(userId, model.Base64Image);
                await _service.AddAsync(userSignature);
            }
            else
            {
                userSignature.Base64Image = model.Base64Image;
                await _service.UpdateAsync(userSignature);
            }

            return StatusCode(202);
        }
    }
}