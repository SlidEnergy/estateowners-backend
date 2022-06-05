using EstateOwners.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public class CarsService : ICarsService
    {
        private IApplicationDbContext _context;

        public CarsService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Car> AddAsync(string userId, Car car)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            _context.Cars.Add(car);

            _context.TrusteeCars.Add(new TrusteeCar(user, car));
            await _context.SaveChangesAsync();

            return car;
        }

        public async Task<List<Car>> GetListWithAccessCheckAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.GetCarListWithAccessCheckAsync(userId);

        }
    }
}
