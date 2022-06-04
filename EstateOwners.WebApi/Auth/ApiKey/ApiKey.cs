using AspNetCore.Authentication.ApiKey;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EstateOwners.WebApi.Auth
{
    class ApiKey : IApiKey
	{
		public ApiKey(string key, string owner, IEnumerable<Claim> claims = null)
		{
			Key = key;
			OwnerName = owner;
			Claims = claims.ToList() ?? new List<Claim>();
		}

		public string Key { get; }
		public string OwnerName { get; }
		public IReadOnlyCollection<Claim> Claims { get; }
	}
}
