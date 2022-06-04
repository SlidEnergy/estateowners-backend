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
			CreateMap<RegisterBindingModel, ApplicationUser>()
				.ForMember(dest => dest.UserName,
					opt => opt.MapFrom(src => src.Email))
				.ForMember(dest => dest.Email,
					opt => opt.MapFrom(src => src.Email))
				.ForAllOtherMembers(opt => opt.Ignore());

			CreateMap<ApplicationUser, Dto.User>()
				.ForMember(dest => dest.IsAdmin,
					opt => opt.Ignore());
		}
	}
}
