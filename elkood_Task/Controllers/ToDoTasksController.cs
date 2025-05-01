using ElKood.Application.Interfaces;
using ElKood.Application.Shared.Models.Inputs;
using ElKood.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace elkood_Task.Controllers
{
    [Authorize(Roles = "Owner,Guest")]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTasksController : ControllerBase
    {
        private readonly IToDoTasksService _toDoTasksService;

        public ToDoTasksController(IToDoTasksService toDoTasksService)
        {
            _toDoTasksService = toDoTasksService;
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

                var result = await _toDoTasksService.GetById(id);
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

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllToDoTasks(FilterInput filterInput)
        {
            var result = await _toDoTasksService.GetAll(filterInput);
            return Ok(result);
        }

        [HttpGet("get-by-category/{id}")]
        public async Task<IActionResult> GetByCategoryId()
        {
            try
            {
                var idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!int.TryParse(idStr, out int id))
                {
                    return BadRequest("Invalid ID format.");
                }

                var result = await _toDoTasksService.GetByCategoryId(id);
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

        [Authorize(Roles = "Owner")]
        [HttpPost("add")]
        public async Task<IActionResult> AddToDoTask(ToDoTaskInput toDoTaskInput)
        {
            try
            {
                await _toDoTasksService.AddToDoTask(toDoTaskInput);
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

        [Authorize(Roles = "Owner")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteToDoTask()
        {
            try
            {
                var idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!int.TryParse(idStr, out int id))
                {
                    return BadRequest("Invalid ID format.");
                }
                await _toDoTasksService.DeleteToDoTask(id);
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

        [HttpGet("complete/{id}")]
        public async Task<IActionResult> CompleteToDoTask()
        {
            try
            {
                var idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!int.TryParse(idStr, out int id))
                {
                    return BadRequest("Invalid ID format.");
                }
                await _toDoTasksService.CompleteToDoTask(id);
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

        [HttpGet("uncomplete/{id}")]
        public async Task<IActionResult> UncompleteToDoTask()
        {
            try
            {
                var idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!int.TryParse(idStr, out int id))
                {
                    return BadRequest("Invalid ID format.");
                }
                await _toDoTasksService.UncompleteToDoTask(id);
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

        [Authorize(Roles = "Owner")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateToDoTask(ToDoTaskInput toDoTaskInput)
        {
            try
            {
                var idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!int.TryParse(idStr, out int id))
                {
                    return BadRequest("Invalid ID format.");
                }

                await _toDoTasksService.UpdateToDoTask(id, toDoTaskInput);
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
