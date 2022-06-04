using EstateOwners.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public class EstatesService : IEstatesService
    {
        private IApplicationDbContext _context;

        public EstatesService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Estate> AddEstateAsync(string userId, Estate estate)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var building = await _context.Buildings.Where(x => x.Id == estate.BuildingId).FirstOrDefaultAsync();

            if (building == null)
                return null;

            _context.Estates.Add(estate);

            await _context.SaveChangesAsync();

            _context.TrusteeEstates.Add(new TrusteeEstate(user, estate));
            await _context.SaveChangesAsync();

            return estate;
        }

        public async Task<List<Estate>> GetListWithAccessCheckAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.GetEstateListWithAccessCheckAsync(userId);

        }
    }
}
