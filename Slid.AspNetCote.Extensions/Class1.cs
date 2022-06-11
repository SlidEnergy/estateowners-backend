using System;

namespace Slid.AspNetCote.Extensions
{
    public static class EnvironmentExtensions
    {
        public static bool IsDevelopment(this Environment environment)
        {
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        }
    }
}
