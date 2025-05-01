using AutoMapper;
using ElKood.Application.Interfaces;
using ElKood.Application.Shared.Models.Dtos;
using ElKood.Application.Shared.Models.Inputs;
using ElKood.Core.Abstractions;
using ElKood.Core.Entities;
using ElKood.Core.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ElKood.Application.Extensions;

namespace ElKood.Application.Services
{
    public class ToDoTasksService : IToDoTasksService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ToDoTaskInput> _validator;
        private readonly IMapper _mapper;

        public ToDoTasksService(IRepository<TaskCategory> taskCategoriesRepository, IRepository<ToDoTask> toDoTasksRepository, IValidator<ToDoTaskInput> validator, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task AddToDoTask(ToDoTaskInput ToDoTaskInput)
        {
            var validationResult = _validator.Validate(ToDoTaskInput);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var toDoTaskDto = new ToDoTaskDto();
            var task = await _unitOfWork.Tasks.GetAll().FirstOrDefaultAsync(tc => tc.Name == ToDoTaskInput.Name);
            if (task != null)
            {
                throw new UserFriendlyException("Task Category already exists.", HttpStatusCode.BadRequest);
            }

            var toDoTask = new ToDoTask
            {
                Name = ToDoTaskInput.Name,
                Description = ToDoTaskInput.Description,
                Priority = ToDoTaskInput.Priority,
                CategoryId = ToDoTaskInput.CategoryId,
            };

            await _unitOfWork.Tasks.AddAsync(toDoTask);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ToDoTaskDto>> GetAll(FilterInput filterInput)
        {
            var tasks = _unitOfWork.Tasks.GetAll().Include(tc => tc.Category).Select(t => t);
            if (!string.IsNullOrEmpty(filterInput.Filter))
            {
                tasks = tasks?.Where(x => x.Name.ToLower().Contains(filterInput.Filter.ToLower())
                || !string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(filterInput.Filter.ToLower()));
            }

            var dataSize = tasks.Count();
            if (filterInput?.SortDirection == "desc")
            {
                tasks = tasks.OrderByDescending(filterInput.SortActive ?? "Id");
            }
            else
            {
                tasks = tasks?.OrderBy(filterInput.SortActive ?? "Id");
            }

            var result = await tasks.Skip(filterInput.PageSize * filterInput.PageIndex).Take(filterInput.PageSize).ToListAsync();
            return _mapper.Map<List<ToDoTaskDto>>(tasks);
        }

        public async Task<List<ToDoTaskDto>> GetByCategoryId(int id)
        {
            var taskCategory = await _unitOfWork.TaskCategories.GetAll().FirstOrDefaultAsync(tc => tc.Id == id);
            if (taskCategory == null)
            {
                throw new UserFriendlyException("Task category not found.", HttpStatusCode.NotFound);
            }

            var toDoTasks = _unitOfWork.Tasks.GetAll().Include(tc => tc.Category).Where(t => t.CategoryId == taskCategory.Id && !t.IsDeleted).ToList();
            if (toDoTasks.Count == 0)
            {
                throw new UserFriendlyException("To-Do Tasks not found.", HttpStatusCode.NotFound);
            }
            ;
            return _mapper.Map<List<ToDoTaskDto>>(toDoTasks);
        }

        public async Task<ToDoTaskDto> GetById(int id)
        {
            var toDoTask = await _unitOfWork.Tasks.GetAll().Include(tc => tc.Category).FirstOrDefaultAsync(t => t.Id == id);
            if (toDoTask == null)
            {
                throw new UserFriendlyException("To-Do Task not found.", HttpStatusCode.NotFound);
            }
            return _mapper.Map<ToDoTaskDto>(toDoTask);
        }

        public async Task<ToDoTaskDto> UpdateToDoTask(int id, ToDoTaskInput toDoTaskInput)
        {
            var validationResult = _validator.Validate(toDoTaskInput);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            if (id != toDoTaskInput.Id)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var task = await _unitOfWork.Tasks.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                throw new UserFriendlyException("To-Do Task not found.", HttpStatusCode.NotFound);
            }

            task.Name = toDoTaskInput.Name;
            task.Description = toDoTaskInput.Description;
            task.Priority = toDoTaskInput.Priority;
            task.CategoryId = toDoTaskInput.CategoryId;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ToDoTaskDto>(task);
        }

        public async Task CompleteToDoTask(int id)
        {
            var toDoTask = await _unitOfWork.Tasks.GetAll().Include(tc => tc.Category).FirstOrDefaultAsync(t => t.Id == id);
            if (toDoTask == null)
            {
                throw new UserFriendlyException("To-Do Task not found.", HttpStatusCode.NotFound);
            }

            toDoTask.IsCompleted = true;
            toDoTask.CompletionDate = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UncompleteToDoTask(int id)
        {
            var toDoTask = await _unitOfWork.Tasks.GetAll().Include(tc => tc.Category).FirstOrDefaultAsync(t => t.Id == id);
            if (toDoTask == null)
            {
                throw new UserFriendlyException("To-Do Task not found.", HttpStatusCode.NotFound);
            }

            toDoTask.IsCompleted = false;
            toDoTask.CompletionDate = null;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteToDoTask(int id)
        {
            var toDoTaskDto = new ToDoTaskDto();
            var task = await _unitOfWork.Tasks.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                throw new UserFriendlyException("To-Do task not found.", HttpStatusCode.NotFound);
            }
            task.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
