using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EstateOwners.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App
{
	public interface IApplicationDbContext
	{
		DbSet<AuthToken> AuthTokens { get; set; }
		DbSet<ApplicationUser> Users { get; set; }

		DbSet<IdentityRole> Roles { get; set; }

		DbSet<IdentityUserRole<string>> UserRoles { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}