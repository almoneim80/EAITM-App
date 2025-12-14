using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Commands.TaskCMD;
using MediatR;

namespace EAITMApp.Application.Handlers.TaskHDL
{
    public class DeleteTodoTaskHandler : IRequestHandler<DeleteTodoTaskCommand, bool>
    {
        private readonly IWriteTodoTaskRepository _writeRepository;
        private readonly IReadTodoTaskRepository _readRepository;

        public DeleteTodoTaskHandler(IWriteTodoTaskRepository writeRepository, IReadTodoTaskRepository readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        public async Task<bool> Handle(DeleteTodoTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _readRepository.GetByIdAsync(request.Id);
            if (task == null)
                return false;

            await _writeRepository.DeleteAsync(request.Id);
            return true;
        }
    }
}
