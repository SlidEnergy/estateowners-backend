using Moq;
using NUnit.Framework;
using EstateOwners.App;
using EstateOwners.Domain;
using System.Threading.Tasks;
using EstateOwners.WebApi;
using EstateOwners.WebApi.Telegram.Connect;
using Microsoft.Extensions.Options;

namespace EstateOwners.UnitTests
{
    public class TelegramServiceTests : TestsBase
	{
		private TelegramService _service;
		private Mock<IAuthTokenService> _authTokenService;
		private Mock<ITokenService> _tokenService;
		private Mock<IUsersService> _usersService;

		[SetUp]
        public void Setup()
        {
			var botSettings = SettingsFactory.CreateTelegramBot();
			_authTokenService = new Mock<IAuthTokenService>();
			_tokenService = new Mock<ITokenService>();
			_usersService = new Mock<IUsersService>();

			_service = new TelegramService(_authTokenService.Object, Options.Create<TelegramBotSettings>(botSettings), _usersService.Object, _tokenService.Object);

		}

		[Test]
		public async Task ValidateTelegramData_ShouldNotBeException()
		{
			_authTokenService.Setup(x => x.AddToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<AuthTokenType>())).Returns(Task.CompletedTask);

			var telegramUser = new WebApi.Telegram.TelegramUser()
			{
				Auth_date = 1575256110,
				First_name = "FirstName",
				Hash = "74035677c5d604d9498b2d20938b40e6c55c717cd15822aa2c2d3e1f056dba51",
				Id = 123456789,
				Last_name = "LastName",
				Username = "Username"
			};

			await _service.ConnectTelegramUser(_user.Id, telegramUser);

			_authTokenService.Verify(x => x.AddToken(
				It.Is<string>(u => u == _user.Id), 
				It.Is<string>(t => t == telegramUser.Id.ToString()), 
				It.Is<AuthTokenType>(t=>t == AuthTokenType.TelegramUserId)));
		}
	}
}