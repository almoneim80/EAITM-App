using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Commands.TaskCMD;
using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.Handlers.TaskHDL
{
    public class AddTodoTaskHandler : IRequestHandler<AddTodoTaskCommand, TodoTask>
    {
        private readonly IWriteTodoTaskRepository _repository;

        public AddTodoTaskHandler(IWriteTodoTaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<TodoTask> Handle(AddTodoTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new TodoTask(request.Title, request.Description);

            await _repository.AddAsync(task);

            return task;
        }
    }
}
