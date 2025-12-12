using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Commands.TaskCMD;
using MediatR;

namespace EAITMApp.Application.Handlers.TaskHDL
{
    public class DeleteTodoTaskHandler : IRequestHandler<DeleteTodoTaskCommand, bool>
    {
        private readonly IReadTodoTaskRepository _repository;

        public DeleteTodoTaskHandler(IReadTodoTaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteTodoTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _repository.GetByIdAsync(request.Id);
            if (task == null)
                return false;

            await _repository.DeleteAsync(request.Id);
            return true;
        }
    }
}
