using Microsoft.AspNetCore.Identity;
using EstateOwners.App;
using EstateOwners.Domain;
using System.Collections.Generic;
using System.Security.Claims;

namespace EstateOwners.WebApi
{
    public interface IClaimsGenerator
    {
        IEnumerable<Claim> CreateClaims(ApplicationUser user, IEnumerable<string> roles);
    }
}
