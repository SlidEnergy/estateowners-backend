using EstateOwners.Domain;
using EstateOwners.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EstateOwners.WebApi.UnitTests
{
	public static class EntityFactoryExtensions
	{
		public static async Task<ApplicationUser> CreateUser(this ApplicationDbContext db)
		{
			var trustee = new Trustee();
			db.Trustee.Add(trustee);
			await db.SaveChangesAsync();
			var user = new ApplicationUser() { Email = Guid.NewGuid().ToString(), TrusteeId = trustee.Id};
			db.Users.Add(user);
			await db.SaveChangesAsync();

			return user;
		}
	}
}
