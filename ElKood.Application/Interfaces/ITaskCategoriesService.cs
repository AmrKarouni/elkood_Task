using ElKood.Application.Shared.Models.Dtos;
using ElKood.Application.Shared.Models.Inputs;

namespace ElKood.Application.Interfaces
{
    public interface ITaskCategoriesService
    {
        Task<TaskCategoryDto> GetById(int id);

        Task<List<TaskCategoryDto>> GetAll(FilterInput filterInput);

        Task AddTaskCategory(TaskCategoryInput taskCategoryInput);

        Task<TaskCategoryDto> UpdateTaskCategory(int id, TaskCategoryInput taskCategoryInput);

        Task DeleteTaskCategory(int id);
    }
}
