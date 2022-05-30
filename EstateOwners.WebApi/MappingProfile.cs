using AutoMapper;
using SaltEdgeNetCore.Models.Transaction;
using EstateOwners.App;
using EstateOwners.App.Utils;
using EstateOwners.Domain;
using EstateOwners.Infrastructure;
using System;
using System.Linq;

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
