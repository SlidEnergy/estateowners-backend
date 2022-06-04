using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using EstateOwners.App;
using EstateOwners.Domain;
using System.Threading.Tasks;
using EstateOwners.WebApi;

namespace EstateOwners.UnitTests
{
	public class UsersServiceTests: TestsBase
	{
		UsersService _service;
		Mock<UserManager<ApplicationUser>> _manager;
		Mock<IAuthTokenService> _authTokenService;

		[SetUp]
		public void Setup()
		{
			var authSettings = SettingsFactory.CreateAuth();
			var claimsGenerator = new ClaimsGenerator(Options.Create(new IdentityOptions()));
			var tokenGenerator = new TokenGenerator(authSettings, claimsGenerator);
			var store = new Mock<IUserStore<ApplicationUser>>();

			_manager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
			_authTokenService = new Mock<IAuthTokenService>();

			_service = new UsersService(_manager.Object, _authTokenService.Object, _db);
		}

		[Test]
		public async Task Register_ShouldBeCallAddMethodWithRightArguments()
		{
			_manager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

			var password = "Password1#";

			var result = await _service.CreateUserAsync(_user, password);

			_manager.Verify(x => x.CreateAsync(
				It.Is<ApplicationUser>(u=> u.UserName == _user.UserName && u.Email == _user.Email), 
				It.Is<string>(p=>p == password)), Times.Exactly(1));
		}

		[Test]
		public async Task RegisterByToken_ShouldBeCallAddMethodWithRightArguments()
		{
			_manager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
			_authTokenService.Setup(x => x.AddToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<AuthTokenType>())).Returns(Task.CompletedTask);

			var token = "23423423";
			var tokenType = AuthTokenType.TelegramUserId;

			var result = await _service.CreateUserAsync(_user, token, tokenType);

			_manager.Verify(x => x.CreateAsync(It.Is<ApplicationUser>(u => u.UserName == _user.UserName && u.Email == _user.Email)), Times.Exactly(1));

			_authTokenService.Verify(x => x.AddToken(It.IsAny<string>(), It.Is<string>(x => x == token), It.Is<AuthTokenType>(x => x == tokenType)),
				Times.Exactly(1));
		}
	}
}