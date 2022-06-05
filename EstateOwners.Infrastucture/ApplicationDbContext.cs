using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EstateOwners.App;
using EstateOwners.Domain;
using EstateOwners.Domain.Signing;
using EstateOwners.Domain.Candidates;

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

            modelBuilder.Entity<TrusteeEstate>()
                .HasKey(key => new { key.EstateId, key.TrusteeId });

            modelBuilder.Entity<TrusteeCar>()
                .HasKey(key => new { key.CarId, key.TrusteeId });


            modelBuilder.Entity<VoteForCandidate>()
                .HasKey(key => new { key.UserId, key.CandidateId });

            modelBuilder.Entity<Candidate>()
                .HasIndex(x => new { x.UserId, x.Type }).IsUnique();
        }

        public DbSet<AuthToken> AuthTokens { get; set; }

        public DbSet<Trustee> Trustee { get; set; }

        public DbSet<TrusteeEstate> TrusteeEstates { get; set; }
        public DbSet<TrusteeCar> TrusteeCars { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Estate> Estates { get; set; }

        public DbSet<Building> Buildings { get; set; }

        public DbSet<ResidentialComplex> ResidentialComplexes { get; set; }

        public DbSet<MessageToSign> MessagesToSign { get; set; }

        public DbSet<UserSignature> UserSignatures { get; set; }

        public DbSet<Poll> Polls { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<VoteForCandidate> VotesForCandidates { get; set; }
        public DbSet<TelegramUser> TelegramUsers { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}
