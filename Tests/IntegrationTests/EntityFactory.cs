using EstateOwners.Domain;
using EstateOwners.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EstateOwners.WebApi.IntegrationTests
{
	public static class EntityFactoryExtensions
	{
		public static async Task<ApplicationUser> CreateUser(this ApplicationDbContext db)
		{
			var user = new ApplicationUser(new Trustee(), Guid.NewGuid().ToString());
			db.Users.Add(user);
			await db.SaveChangesAsync();

			return user;
		}
	}
}
