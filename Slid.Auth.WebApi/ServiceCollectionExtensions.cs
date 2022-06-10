using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slid.Auth.WebApi
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSlidAuth(this IServiceCollection services)
        {
            return services;
        }
    }
}
