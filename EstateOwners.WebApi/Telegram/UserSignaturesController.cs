using EstateOwners.App;
using EstateOwners.App.Telegram;
using EstateOwners.Domain.Telegram;
using EstateOwners.TelegramBot.Dialogs;
using EstateOwners.WebApi.Telegram.Connect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Slid.Security;
using System;
using System.Threading.Tasks;

namespace EstateOwners.WebApi.Telegram
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserSignaturesController : ControllerBase
    {
        private readonly IUserSignaturesService _service;
        private readonly IUsersService _usersService;
        private readonly TelegramBotSettings _telegramBotSettings;

        public UserSignaturesController(IUserSignaturesService service, IOptions<TelegramBotSettings> options, IUsersService usersService)
        {
            _service = service;
            _usersService = usersService;
            _telegramBotSettings = options.Value;
        }


        [HttpPost()]
        public async Task<ActionResult> AddUserSignature(UserSignatureBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var key = _telegramBotSettings.Aes256Key.Substring(0, _telegramBotSettings.Aes256Key.Length - model.Salt.Length) + model.Salt;
            var json = Aes256.DecryptString(key, Uri.UnescapeDataString(model.Payload));
            var payload = JsonConvert.DeserializeObject<TelegramGamePayload>(json);

            var user = await _usersService.GetByAuthTokenAsync(payload.ChatId.ToString(), Domain.AuthTokenType.TelegramUserId);

            var userId = user.Id;

            var userSignature = await _service.GetByUserAsync(userId);

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