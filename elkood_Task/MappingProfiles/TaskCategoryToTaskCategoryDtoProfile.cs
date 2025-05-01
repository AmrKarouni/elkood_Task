using AutoMapper;
using ElKood.Application.Shared.Models.Dtos;
using ElKood.Core.Entities;

namespace elkood_Task.MappingProfiles
{
    public class TaskCategoryToTaskCategoryDtoProfile : Profile
    {
        public TaskCategoryToTaskCategoryDtoProfile()
        {
            CreateMap<TaskCategory, TaskCategoryDto>();
        }
    }
}
