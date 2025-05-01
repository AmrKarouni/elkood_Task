using AutoMapper;
using ElKood.Application.Shared.Models.Dtos.Identity;
using ElKood.Core.Entities.Identity;

namespace elkood_Task.MappingProfiles
{
    public class ApplicationUserToUserDtoProfile : Profile
    {
        public ApplicationUserToUserDtoProfile()
        {
            CreateMap<ApplicationUser, UserDto>().ForMember(u => u.Id, au => au.MapFrom(a => a.Id))
                .ForMember(u => u.UserName, au => au.MapFrom(a => a.UserName))
                .ForMember(u => u.Email, au => au.MapFrom(a => a.Email));
        }
    }
}
