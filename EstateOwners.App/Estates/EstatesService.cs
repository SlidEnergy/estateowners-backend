using EstateOwners.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        public async Task<Estate> AddEstate(string userId, int buildingId, EstateType type, string number)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var building = await _context.Buildings.Where(x => x.Id == buildingId).FirstOrDefaultAsync();

            if (building == null)
                return null;

            var estate = new Estate(type, buildingId, number);
            _context.Estates.Add(estate);

            await _context.SaveChangesAsync();

            _context.TrusteeEstates.Add(new TrusteeEstate(user, estate));
            await _context.SaveChangesAsync();

            return estate;
        }
    }
}
