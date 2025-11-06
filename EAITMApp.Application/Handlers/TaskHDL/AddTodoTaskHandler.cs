using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Commands.TaskCMD;
using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.Handlers.TaskHDL
{
    public class AddTodoTaskHandler : IRequestHandler<AddTodoTaskCommand, TodoTask>
    {
        private readonly IEnumerable<ITodoTaskRepository> _repositories;

        public AddTodoTaskHandler(IEnumerable<ITodoTaskRepository> repositories)
        {
            _repositories = repositories;
        }

        public async Task<TodoTask> Handle(AddTodoTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new TodoTask(request.Title, request.Description);

            // إضافة المهمة في كل مصدر تخزين موجود
            foreach (var repo in _repositories)
            {
                await repo.AddAsync(task);
            }

            return task;
        }
    }
}
