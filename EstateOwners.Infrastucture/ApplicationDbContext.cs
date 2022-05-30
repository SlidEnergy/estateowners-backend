using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EstateOwners.App;
using EstateOwners.Domain;

namespace EstateOwners.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AuthToken> AuthTokens { get; set; }

        public DbSet<Trustee> Trustee { get; set; }

		public DbSet<ApplicationUser> Users { get; set; }
    }
}
