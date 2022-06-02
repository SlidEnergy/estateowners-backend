using EstateOwners.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public static class ApplicationDbContextExtensions
	{
		public static async Task<bool> IsAdmin(this IApplicationDbContext context, string userId)
		{
			var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == Role.Admin);

			if (role == null)
				throw new Exception("Роль администратора не найдена.");

			var userRole = await context.UserRoles.FirstOrDefaultAsync(x => x.RoleId == role.Id && x.UserId == userId);

			return userRole == null ? false : true;
		}
	}
}
