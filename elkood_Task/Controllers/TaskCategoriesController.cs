using ElKood.Application.Interfaces;
using ElKood.Application.Shared.Models.Inputs;
using ElKood.Core.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace elkood_Task.Controllers
{
    [Authorize(Roles = "Owner")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskCategoriesController : ControllerBase
    {
        private readonly ITaskCategoriesService _taskCategoriesService;

        public TaskCategoriesController(ITaskCategoriesService taskCategoriesService)
        {
            _taskCategoriesService = taskCategoriesService;
        }


        [Authorize]
        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllTaskCategories(FilterInput filterInput)
        {
            var result = await _taskCategoriesService.GetAll(filterInput);
            return Ok(result);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById()
        {
            try
            {
                var idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!int.TryParse(idStr, out int id))
                {
                    return BadRequest("Invalid ID format.");
                }

                var result = await _taskCategoriesService.GetById(id);
                return Ok(result);
            }
            catch (UserFriendlyException ex)
            {
                if (ex.Code == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(ex.Message);
                }

                return BadRequest(ex.Message);
            }

        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTaskCategory(TaskCategoryInput taskCategoryInput)
        {
            try
            {
                await _taskCategoriesService.AddTaskCategory(taskCategoryInput);
                return Created();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserFriendlyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTaskCategory()
        {
            try
            {
                var idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!int.TryParse(idStr, out int id))
                {
                    return BadRequest("Invalid ID format.");
                }
                await _taskCategoriesService.DeleteTaskCategory(id);
                return Ok();
            }
            catch (UserFriendlyException ex)
            {
                if (ex.Code == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTaskCategory(TaskCategoryInput taskCategoryInput)
        {
            try
            {
                var idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!int.TryParse(idStr, out int id))
                {
                    return BadRequest("Invalid ID format.");
                }

                await _taskCategoriesService.UpdateTaskCategory(id, taskCategoryInput);
                return Ok();
            }
            catch (UserFriendlyException ex)
            {
                if (ex.Code == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
