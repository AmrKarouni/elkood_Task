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
    public class TaskCategoriesService : ITaskCategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<TaskCategoryInput> _validator;
        private readonly IMapper _mapper;

        public TaskCategoriesService(IValidator<TaskCategoryInput> validator, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddTaskCategory(TaskCategoryInput taskCategoryInput)
        {
            var validationResult = _validator.Validate(taskCategoryInput);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var taskCategoryDto = new TaskCategoryDto();
            var category = await _unitOfWork.TaskCategories.GetAll().FirstOrDefaultAsync(tc => tc.Name == taskCategoryInput.Name);
            if (category != null)
            {
                throw new UserFriendlyException("Task Category already exists.", HttpStatusCode.BadRequest);
            }

            var taskCategory = new TaskCategory
            {
                Name = taskCategoryInput.Name,
                Description = taskCategoryInput.Description,
            };

            await _unitOfWork.TaskCategories.AddAsync(taskCategory);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTaskCategory(int id)
        {
            var taskCategoryDto = new TaskCategoryDto();
            var category = await _unitOfWork.TaskCategories.GetAll().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (category == null)
            {
                throw new UserFriendlyException("Task Category not found.", HttpStatusCode.NotFound);
            }
            category.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<TaskCategoryDto>> GetAll(FilterInput filterInput)
        {
            var taskCategories = _unitOfWork.TaskCategories.GetAll();
            if (!string.IsNullOrEmpty(filterInput.Filter))
            {
                taskCategories = taskCategories?.Where(x => x.Name.ToLower().Contains(filterInput.Filter.ToLower())
                || !string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(filterInput.Filter.ToLower()));
            }

            var dataSize = taskCategories.Count();
            if (filterInput?.SortDirection == "desc")
            {
                taskCategories = taskCategories?.OrderByDescending(filterInput.SortActive ?? "Id");
            }
            else
            {
                taskCategories = taskCategories?.OrderBy(filterInput.SortActive ?? "Id");
            }

            var result = await taskCategories.Skip(filterInput.PageSize * filterInput.PageIndex).Take(filterInput.PageSize).ToListAsync();
            return _mapper.Map<List<TaskCategoryDto>>(taskCategories);
        }

        public async Task<TaskCategoryDto> GetById(int id)
        {
            var taskCategory = await _unitOfWork.TaskCategories.GetAll().FirstOrDefaultAsync(tc => tc.Id == id);
            if (taskCategory == null)
            {
                throw new UserFriendlyException("Task category not found.", HttpStatusCode.NotFound);
            }
            return _mapper.Map<TaskCategoryDto>(taskCategory);
        }

        public async Task<TaskCategoryDto> UpdateTaskCategory(int id, TaskCategoryInput taskCategoryInput)
        {
            var validationResult = _validator.Validate(taskCategoryInput);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            if (id != taskCategoryInput.Id)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var taskCategory = await _unitOfWork.TaskCategories.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (taskCategory == null)
            {
                throw new UserFriendlyException("Task category not found.", HttpStatusCode.NotFound);
            }

            taskCategory.Name = taskCategoryInput.Name;
            taskCategory.Description = taskCategoryInput.Description;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TaskCategoryDto>(taskCategory);
        }
    }
}
