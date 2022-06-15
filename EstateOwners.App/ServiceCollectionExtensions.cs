using EstateOwners.App.Polls;
using EstateOwners.App.Signing;
using EstateOwners.App.Telegram;
using EstateOwners.App.Telegram.Documents;
using EstateOwners.App.Telegram.Support;
using EstateOwners.App.Telegram.Voting;
using Microsoft.Extensions.DependencyInjection;

namespace EstateOwners.App
{
    public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddEstateOwnersCore(this IServiceCollection services)
		{
			services.AddScoped<IUsersService, UsersService>();
			services.AddScoped<IAuthTokenService, AuthTokenService>();
			services.AddScoped<IEstatesService, EstatesService>();
			services.AddScoped<ICarsService, CarsService>();
			services.AddScoped<IBuildingsService, BuildingsService>();
			services.AddScoped<IVoteTelegramMessagesService, VoteTelegramMessagesService>();
			services.AddScoped<IDocumentTelegramMessagesService, DocumentTelegramMessagesService>();
			services.AddScoped<IPollsService, PollsService>();
			services.AddScoped<ICandidatesService, CandidatesService>();
			services.AddScoped<IExportService, ExportService>();
			services.AddScoped<IDocumentTelegramMessagesService, DocumentTelegramMessagesService>();
			services.AddScoped<IIssueTelegramMessagesService, IssueTelegramMessagesService>();
			services.AddScoped<IUserSignaturesService, UserSignaturesService>();

			return services;
		}
	}
}
