using EAITMApp.Application.Exceptions;
using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Commands.TaskCMD;
using EAITMApp.Domain.Entities;
using EAITMApp.SharedKernel.Errors.Registries;
using EAITMApp.SharedKernel.Exceptions;
using MediatR;

namespace EAITMApp.Application.Handlers.TaskHDL
{
    public class AddTodoTaskHandler : IRequestHandler<AddTodoTaskCommand, TodoTask>
    {
        private readonly IWriteTodoTaskRepository _repository;
        private readonly IReadTodoTaskRepository _readRepository;

        public AddTodoTaskHandler(IWriteTodoTaskRepository repository, IReadTodoTaskRepository readRepository)
        {
            _repository = repository;
            _readRepository = readRepository;
        }

        public async Task<TodoTask> Handle(AddTodoTaskCommand request, CancellationToken cancellationToken)
        {
            if (_readRepository.ExistsByTitleAsync(request.Title, cancellationToken))
                throw new ConflictException(TaskErrors.DuplicateTitle);

            var task = new TodoTask(request.Title, request.Description);
            await _repository.AddAsync(task);
            return task;
        }
    }
}
