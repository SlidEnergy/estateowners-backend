using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EstateOwners.TelegramBot.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class MessagesController : ControllerBase
	{
		[HttpPost, HttpGet]
		public async Task PostAsync()
		{
		}
	}
}
