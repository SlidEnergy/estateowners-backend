using AutoMapper;
using EstateOwners.Domain;
using EstateOwners.Infrastructure;
using Telegram.Bot.Types;

namespace EstateOwners.WebApi
{
    public class MappingProfile : Profile
	{
		public MappingProfile(ApplicationDbContext context)
		{
			CreateMap<ApplicationUser, Dto.User>()
				.ForMember(dest => dest.IsAdmin,
					opt => opt.Ignore());

			CreateMap<Dto.User, ApplicationUser>();
		}
	}
}
