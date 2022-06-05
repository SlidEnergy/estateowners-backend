using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EstateOwners.Domain;
using System.Threading;
using System.Threading.Tasks;
using EstateOwners.Domain.Signing;
using EstateOwners.Domain.Candidates;

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

		DbSet<MessageToSign> MessagesToSign { get; set; }

		DbSet<UserSignature> UserSignatures { get; set; }

		DbSet<Poll> Polls { get; set; }

        DbSet<Candidate> Candidates { get; set; }

		DbSet<VoteForCandidate> VotesForCandidates { get; set; }

		DbSet<TelegramUser> TelegramUsers { get; set; }
		DbSet<Car> Cars { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}