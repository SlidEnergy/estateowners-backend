using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EstateOwners.Domain;
using System.Threading;
using System.Threading.Tasks;
using EstateOwners.Domain.Candidates;
using EstateOwners.Domain.Telegram;
using EstateOwners.Domain.Telegram.Voting;
using EstateOwners.Domain.Telegram.Support;

namespace EstateOwners.App
{
    public interface IApplicationDbContext
	{
		DbSet<Trustee> Trustee { get; set; }
		DbSet<TrusteeEstate> TrusteeEstates { get; set; }
		DbSet<TrusteeCar> TrusteeCars { get; set; }

		DbSet<AuthToken> AuthTokens { get; set; }
		DbSet<ApplicationUser> Users { get; set; }

		DbSet<IdentityRole> Roles { get; set; }

		DbSet<IdentityUserRole<string>> UserRoles { get; set; }

		DbSet<Estate> Estates { get; set; }

		DbSet<Building> Buildings{ get; set; }

		DbSet<ResidentialComplex> ResidentialComplexes { get; set; }

		DbSet<VoteTelegramMessage> VoteTelegramMessages { get; set; }
		
		DbSet<DocumentTelegramMessage> DocumentTelegramMessages { get; set; }
		
		DbSet<IssueTelegramMessage> IssueTelegramMessages { get; set; }

		DbSet<TelegramMessageVote> UserMessageVotes { get; set; }

		DbSet<UserSignature> UserSignatures { get; set; }

		DbSet<Poll> Polls { get; set; }

        DbSet<Candidate> Candidates { get; set; }

		DbSet<UserCandidateVote> CandidateVotes { get; set; }

		DbSet<TelegramUser> TelegramUsers { get; set; }
		DbSet<Car> Cars { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}