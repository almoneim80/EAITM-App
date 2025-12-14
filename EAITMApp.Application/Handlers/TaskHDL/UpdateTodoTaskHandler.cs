using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Commands.TaskCMD;
using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.Handlers.TaskHDL
{
    public class UpdateTodoTaskHandler : IRequestHandler<UpdateTodoTaskCommand, TodoTask?>
    {
        private readonly IWriteTodoTaskRepository _repository;
        private readonly IReadTodoTaskRepository _readRepository;

        public UpdateTodoTaskHandler(IWriteTodoTaskRepository repository, IReadTodoTaskRepository readRepository)
        {
            _repository = repository;
            _readRepository = readRepository;
        }

        public async Task<TodoTask?> Handle(UpdateTodoTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _readRepository.GetByIdAsync(request.Id);
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
