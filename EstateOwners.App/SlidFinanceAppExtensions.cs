﻿using Microsoft.Extensions.DependencyInjection;

namespace EstateOwners.App
{
    public static class EstateOwnersAppExtensions
	{
		public static IServiceCollection AddEstateOwnersCore(this IServiceCollection services)
		{
			services.AddScoped<IUsersService, UsersService>();
			services.AddScoped<IAuthTokenService, AuthTokenService>();

			return services;
		}
	}
}
