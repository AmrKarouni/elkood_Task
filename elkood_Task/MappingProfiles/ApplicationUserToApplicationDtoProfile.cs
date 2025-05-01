using AutoMapper;
using ElKood.Application.Shared.Models.Dtos.Identity;
using ElKood.Core.Entities.Identity;

namespace elkood_Task.MappingProfiles
{
    public class ApplicationUserToApplicationDtoProfile : Profile
    {
        public ApplicationUserToApplicationDtoProfile()
        {
            CreateMap<ApplicationUser, AuthenticationDto>()
                .ForMember(u => u.UserName, au => au.MapFrom(a => a.UserName));
        }
    }
}
