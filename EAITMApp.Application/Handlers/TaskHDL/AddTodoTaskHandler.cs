using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Commands.TaskCMD;
using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.Handlers.TaskHDL
{
    public class AddTodoTaskHandler : IRequestHandler<AddTodoTaskCommand, TodoTask>
    {
        private readonly ITodoTaskRepository _repository;
        public AddTodoTaskHandler(ITodoTaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<TodoTask> Handle(AddTodoTaskCommand request, CancellationToken cancellationToken)
        {
            // 1. إنشاء المهمة
            var task = new TodoTask(request.Title, request.Description);

            // 2. حفظ المهمة
            return await _repository.AddAsync(task);
        }
    }
}
