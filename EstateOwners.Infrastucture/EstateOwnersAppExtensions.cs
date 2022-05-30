using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EstateOwners.App;
using EstateOwners.Domain;

namespace EstateOwners.Infrastructure
{
	public static class EstateOwnersAppExtensions
	{
		public static IServiceCollection AddEstateOwnersInfrastructure(this IServiceCollection services, string connectionString)
		{
			services.AddEntityFrameworkNpgsql()
							.AddDbContext<ApplicationDbContext>(options => options
								.UseLazyLoadingProxies()
                                .UseNpgsql(connectionString))
							.BuildServiceProvider();

			services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

			services.AddScoped<IRepository<ApplicationUser, string>, EfRepository<ApplicationUser, string>>();
			services.AddScoped<IAuthTokensRepository, EfAuthTokensRepository>();

			services.AddScoped<DataAccessLayer>();

			return services;
		}
	}
}
