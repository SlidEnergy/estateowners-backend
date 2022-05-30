using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using EstateOwners.App;
using EstateOwners.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EstateOwners.WebApi.UnitTests
{
	public class TokenServiceTests : TestsBase
	{
		private TokenService _service;
		Mock<ITokenGenerator> _tokenGenerator;
		Mock<UserManager<ApplicationUser>> _manager;
		private Mock<IAuthTokenService> _authTokenService;

		[SetUp]
		public void Setup()
		{
			var authSettings = SettingsFactory.CreateAuth();
			_tokenGenerator = new Mock<ITokenGenerator>();
			var store = new Mock<IUserStore<ApplicationUser>>();

			_manager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
			_authTokenService = new Mock<IAuthTokenService>();
			_service = new TokenService(_tokenGenerator.Object, authSettings, _authTokenService.Object, _manager.Object);
		}

		[Test]
		public async Task GenerateAccessAndRefreshTokens_ShouldBeCalledMethods()
		{
			var accessToken = Guid.NewGuid().ToString();
			var refreshToken = Guid.NewGuid().ToString();

			_tokenGenerator.Setup(x => x.GenerateAccessToken(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>())).Returns(accessToken);
			_tokenGenerator.Setup(x => x.GenerateRefreshToken()).Returns(refreshToken);
			_authTokenService.Setup(x => x.AddToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<AuthTokenType>())).Returns(Task.CompletedTask);
			_manager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).Returns(Task.FromResult<IList<string>>(new List<string> { Role.Admin }));

			await _service.GenerateAccessAndRefreshTokens(_user);

			_tokenGenerator.Verify(x => x.GenerateAccessToken(It.Is<ApplicationUser>(user => user.Id == _user.Id), It.Is<IEnumerable<string>>(r => r.Count() == 1)));
			_tokenGenerator.Verify(x => x.GenerateRefreshToken());
			_authTokenService.Verify(x => x.AddToken(It.Is<string>(u => u == _user.Id), It.Is<string>(t => t == refreshToken), It.Is<AuthTokenType>(t => t == AuthTokenType.RefreshToken)));
			_manager.Verify(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u == _user)));
		}


		[Test]
		public async Task RefreshToken_ShouldCalledMethods()
		{
			var authSettings = SettingsFactory.CreateAuth();
			var claimsGenerator = new ClaimsGenerator(Options.Create(new IdentityOptions()));
			var tokenGenerator = new TokenGenerator(authSettings, claimsGenerator);

			var token = tokenGenerator.GenerateAccessToken(_user, new string[] { });
			var refreshToken = tokenGenerator.GenerateRefreshToken();

			var newAccessToken = Guid.NewGuid().ToString();
			var newRefreshToken = Guid.NewGuid().ToString();

			_tokenGenerator.Setup(x => x.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>())).Returns(newAccessToken);
			_tokenGenerator.Setup(x => x.GenerateRefreshToken()).Returns(newRefreshToken);
			_authTokenService.Setup(x => x.FindAnyToken(It.IsAny<string>())).ReturnsAsync(new AuthToken("any", refreshToken, _user, AuthTokenType.RefreshToken));

			await _service.RefreshToken(token, refreshToken);

			_tokenGenerator.Verify(x => x.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>()));
			_tokenGenerator.Verify(x => x.GenerateRefreshToken());
			_authTokenService.Verify(x => x.FindAnyToken(It.Is<string>(t => t == refreshToken)));
		}

		[Test]
		public async Task Login_ShouldBeCallAddMethodWithRightArguments()
		{
			var password = "Password1#";

			_manager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(Task.FromResult(true));
			_manager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(_user));

			var result = await _service.CheckCredentialsAndGetToken(_user.Email, password);

			_manager.Verify(x => x.CheckPasswordAsync(
			  It.Is<ApplicationUser>(u => u.UserName == _user.UserName && u.Email == _user.Email),
			  It.Is<string>(p => p == password)), Times.Exactly(1));
		}
	}
}