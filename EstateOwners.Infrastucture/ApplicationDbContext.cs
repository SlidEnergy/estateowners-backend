using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EstateOwners.App;
using EstateOwners.Domain;
using EstateOwners.Domain.Candidates;
using EstateOwners.Domain.Telegram;
using EstateOwners.Domain.Telegram.Voting;
using EstateOwners.Domain.Telegram.Support;

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


            modelBuilder.Entity<UserCandidateVote>()
                .HasKey(key => new { key.UserId, key.CandidateId });

            modelBuilder.Entity<Candidate>()
                .HasIndex(x => new { x.UserId, x.Type }).IsUnique();

            modelBuilder.Entity<TelegramMessageVote>()
                .HasKey(x => new { x.UserId, x.VoteTelegramMessageId });
        }

        public DbSet<AuthToken> AuthTokens { get; set; }

        public DbSet<Trustee> Trustee { get; set; }

        public DbSet<TrusteeEstate> TrusteeEstates { get; set; }
        public DbSet<TrusteeCar> TrusteeCars { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Estate> Estates { get; set; }

        public DbSet<Building> Buildings { get; set; }

        public DbSet<ResidentialComplex> ResidentialComplexes { get; set; }

        public DbSet<VoteTelegramMessage> VoteTelegramMessages { get; set; }

        public DbSet<DocumentTelegramMessage> DocumentTelegramMessages { get; set; }

        public DbSet<IssueTelegramMessage> IssueTelegramMessages { get; set; }

        public DbSet<TelegramMessageVote> UserMessageVotes { get; set; }
        public DbSet<UserSignature> UserSignatures { get; set; }

        public DbSet<Poll> Polls { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<UserCandidateVote> CandidateVotes { get; set; }
        public DbSet<TelegramUser> TelegramUsers { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}
