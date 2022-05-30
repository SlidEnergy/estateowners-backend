using Microsoft.EntityFrameworkCore;
using Moq;
using EstateOwners.App;
using EstateOwners.Infrastructure;
using NUnit.Framework;
using System.Threading.Tasks;
using EstateOwners.Domain;
using Microsoft.AspNetCore.Identity;
using System;

namespace EstateOwners.UnitTests
{
	public class TestsBase
	{
		protected readonly AutoMapperFactory _autoMapper = new AutoMapperFactory();
		protected ApplicationDbContext _db;
		protected DataAccessLayer _mockedDal;
		protected ApplicationUser _user;

		protected Mock<IRepository<ApplicationUser, string>> _users;
		protected Mock<IAuthTokensRepository> _authTokens;

		[SetUp]
		public async Task SetupBase()
		{
			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			optionsBuilder.UseInMemoryDatabase(TestContext.CurrentContext.Test.Name);
			_db = new ApplicationDbContext(optionsBuilder.Options);

			_users = new Mock<IRepository<ApplicationUser, string>>();
			_authTokens = new Mock<IAuthTokensRepository>();

			_mockedDal = new DataAccessLayer(_users.Object, _authTokens.Object);

			var role = new IdentityRole() { Name = Role.Admin };
			_db.Roles.Add(role);

			var trustee = new Trustee();
			_db.Trustee.Add(trustee);

			var userName = Guid.NewGuid() + "@mail.com";
			_user = new ApplicationUser(trustee, userName);
			_db.Users.Add(_user);
			
			_db.UserRoles.Add(new IdentityUserRole<string>() { RoleId = role.Id, UserId = _user.Id });

			await _db.SaveChangesAsync();
		}
	}
}
