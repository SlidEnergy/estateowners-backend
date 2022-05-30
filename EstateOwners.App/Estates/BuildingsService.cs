using EstateOwners.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public class BuildingsService : IBuildingsService
    {
        private IApplicationDbContext _context;

        public BuildingsService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Building>> GetListAsync()
        {
            return await _context.Buildings.ToListAsync();
        }
    }
}
