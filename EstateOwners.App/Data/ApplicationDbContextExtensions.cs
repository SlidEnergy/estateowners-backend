using EstateOwners.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public static class ApplicationDbContextExtensions
	{
		public static async Task<bool> IsAdminAsync(this IApplicationDbContext context, string userId)
		{
			var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == Role.Admin);

			if (role == null)
				throw new Exception("Роль администратора не найдена.");

			var userRole = await context.UserRoles.FirstOrDefaultAsync(x => x.RoleId == role.Id && x.UserId == userId);

			return userRole == null ? false : true;
		}

        public static async Task<Estate> GetEstateByIdWithAccessCheckAsync(this IApplicationDbContext context, string userId, int id)
        {
			var isAdmin = await context.IsAdminAsync(userId);

            if (isAdmin)
                return await context.Estates.FindAsync(id);

            var user = await context.Users.FindAsync(userId);

            var estate = await context.TrusteeEstates
                .Where(x => x.TrusteeId == user.TrusteeId)
                .Join(context.Estates, t => t.EstateId, e => e.Id, (t, e) => e)
                .FirstOrDefaultAsync(x => x.Id == id);

            return estate;
        }

        public static async Task<List<Estate>> GetEstateListWithAccessCheckAsync(this IApplicationDbContext context,
            string userId, string filterUserId = null)
        {
            var isAdmin = await context.IsAdminAsync(userId);

            if (isAdmin)
            {
                if (filterUserId != null)
                {
                    var filterUser = await context.Users.FindAsync(filterUserId);

                    return await context.TrusteeEstates
                        .Where(x => x.TrusteeId == filterUser.TrusteeId)
                        .Join(context.Estates, t => t.EstateId, e => e.Id, (t, e) => e)
                        .ToListAsync(); 
                }

                return await context.Estates.ToListAsync();
            }

            var user = await context.Users.FindAsync(userId);

            var estates = await context.TrusteeEstates
                .Where(x => x.TrusteeId == user.TrusteeId)
                .Join(context.Estates, t => t.EstateId, e => e.Id, (t, e) => e)
                .ToListAsync();

            return estates;
        }

        public static async Task<List<Car>> GetCarListWithAccessCheckAsync(this IApplicationDbContext context, string userId)
		{
			var user = await context.Users.FindAsync(userId);

			var cars = await context.TrusteeCars
				.Where(x => x.TrusteeId == user.TrusteeId)
				.Join(context.Cars, t => t.CarId, e => e.Id, (t, e) => e)
				.ToListAsync();

			return cars;
		}
	}
}
