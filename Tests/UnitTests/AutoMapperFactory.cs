using AutoMapper;
using EstateOwners.WebApi;
using EstateOwners.Infrastructure;

namespace EstateOwners.WebApi.UnitTests
{
    public class AutoMapperFactory
    {
        public IMapper Create(ApplicationDbContext context)
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile(context)));
            return new Mapper(configuration);
        }
    }
}
