using AutoMapper;
using EstateOwners.Domain;
using EstateOwners.Infrastructure;
using EstateOwners.WebApi.Dto;
using Telegram.Bot.Types;
using Estate = EstateOwners.Domain.Estate;

namespace EstateOwners.WebApi
{
    public class MappingProfile : Profile
	{
		public MappingProfile(ApplicationDbContext context)
		{
			CreateMap<ApplicationUser, Dto.User>()
				.ForMember(dest => dest.IsAdmin,
					opt => opt.Ignore());

            CreateMap<Estate, Dto.Estate>()
                .ForMember(x=> x.Building, opt => opt.MapFrom(b => b.Building.ToString()));


            CreateMap<EstateBindingModel, Estate>();


			CreateMap<Dto.User, ApplicationUser>();
		}
	}
}
