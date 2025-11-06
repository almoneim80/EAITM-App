using EAITMApp.Application.UseCases.Commands.TaskCMD;
using EAITMApp.Application.UseCases.Queries;
using EAITMApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EAITMApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Add a new Todo Task
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TodoTask>> AddTask([FromBody] AddTodoTaskCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Get all Todo Tasks
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<TodoTask>>> GetAllTasks()
        {
            var result = await _mediator.Send(new GetAllTasksQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get task by Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTask>> GetTaskById(Guid id)
        {
            var result = await _mediator.Send(new GetTaskByIdQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Update a task
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoTask>> UpdateTask(Guid id, [FromBody] UpdateTodoTaskCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID in URL does not match ID in body.");

            var result = await _mediator.Send(command);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            var success = await _mediator.Send(new DeleteTodoTaskCommand(id));
            if (!success)
                return NotFound();

            return NoContent();
        }

    }
}
