using AutoMapper;
using ElKood.Application.Shared.Models.Dtos;
using ElKood.Core.Entities;

namespace elkood_Task.MappingProfiles
{
    public class ToDoTaskToToDoTaskDtoProfile : Profile
    {
        public ToDoTaskToToDoTaskDtoProfile()
        {
            CreateMap<ToDoTask, ToDoTaskDto>()
                .ForMember(dest => dest.CategoryName,opt => opt.MapFrom(src => src.Category.Name));

        }
    }
}
