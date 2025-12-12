using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Commands.TaskCMD;
using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.Handlers.TaskHDL
{
    public class UpdateTodoTaskHandler : IRequestHandler<UpdateTodoTaskCommand, TodoTask?>
    {
        private readonly IReadTodoTaskRepository _repository;

        public UpdateTodoTaskHandler(IReadTodoTaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<TodoTask?> Handle(UpdateTodoTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _repository.GetByIdAsync(request.Id);
            if (task == null)
                return null;

            task.UpdateTitle(request.Title);
            task.UpdateDescription(request.Description);

            if (request.IsCompleted != task.IsCompleted)
                task.ToggleTaskCompleted();

            await _repository.UpdateAsync(task);
            return task;
        }
    }
}
