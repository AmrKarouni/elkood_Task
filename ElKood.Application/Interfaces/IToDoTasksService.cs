using ElKood.Application.Shared.Models.Dtos;
using ElKood.Application.Shared.Models.Inputs;

namespace ElKood.Application.Interfaces
{
    public interface IToDoTasksService
    {
        Task<ToDoTaskDto> GetById(int id);

        Task<List<ToDoTaskDto>> GetAll(FilterInput filterInput);

        Task<List<ToDoTaskDto>> GetByCategoryId(int id);

        Task AddToDoTask(ToDoTaskInput ToDoTaskInput);

        Task<ToDoTaskDto> UpdateToDoTask(int id, ToDoTaskInput toDoTaskInput);

        Task CompleteToDoTask(int id);

        Task UncompleteToDoTask(int id);

        Task DeleteToDoTask(int id);
    }
}
