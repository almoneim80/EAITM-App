using EAITMApp.Application.UseCases.Commands.TaskCMD;
using EAITMApp.Application.UseCases.Queries;
using EAITMApp.SharedKernel.Errors.Registries;
using EAITMApp.SharedKernel.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EAITMApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ApiResponseController
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
        public async Task<IActionResult> AddTask([FromBody] AddTodoTaskCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Created(result, "Task created successfully.", nameof(GetTaskById), new { id = result.Id });
        }

        /// <summary>
        /// Get all Todo Tasks
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var result = await _mediator.Send(new GetAllTasksQuery());
            return Success(result, "Task retrieved successfully.");
        }

        /// <summary>
        /// Get task by Id
        /// </summary>
        [HttpGet("{id}", Name = "GetTaskById")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var result = await _mediator.Send(new GetTaskByIdQuery(id));
            return Success(result!, "Task retrieved successfully.");
        }

        /// <summary>
        /// Update a task
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTodoTaskCommand command)
        {
            if (id != command.Id)
                throw new InvalidRequestException(CommonErrors.IdMismatch);

            var result = await _mediator.Send(command);
            return Success(result, "Task updated successfully.");
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var success = await _mediator.Send(new DeleteTodoTaskCommand(id));
            return Success("Task deleted successfully.");
        }

    }
}
