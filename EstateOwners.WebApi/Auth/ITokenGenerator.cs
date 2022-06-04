using Microsoft.AspNetCore.Identity;
using EstateOwners.App;
using EstateOwners.Domain;
using System.Collections.Generic;
using System.Security.Claims;

namespace EstateOwners.WebApi
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(ApplicationUser user, IEnumerable<string> roles, AccessMode accessMode);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }
}
