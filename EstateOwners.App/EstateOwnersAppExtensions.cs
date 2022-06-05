using EstateOwners.App.Polls;
using EstateOwners.App.Signing;
using Microsoft.Extensions.DependencyInjection;

namespace EstateOwners.App
{
    public static class EstateOwnersAppExtensions
	{
		public static IServiceCollection AddEstateOwnersCore(this IServiceCollection services)
		{
			services.AddScoped<IUsersService, UsersService>();
			services.AddScoped<IAuthTokenService, AuthTokenService>();
			services.AddScoped<IEstatesService, EstatesService>();
			services.AddScoped<ICarsService, CarsService>();
			services.AddScoped<IBuildingsService, BuildingsService>();
			services.AddScoped<ISigningService, SigningService>();
			services.AddScoped<IPollsService, PollsService>();
			services.AddScoped<ICandidatesService, CandidatesService>();
			services.AddScoped<IExportService, ExportService>();

			return services;
		}
	}
}
