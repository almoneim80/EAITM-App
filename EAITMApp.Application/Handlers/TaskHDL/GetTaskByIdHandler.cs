using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Queries;
using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.Handlers.TaskHDL
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, TodoTask?>
    {
        private readonly ITodoTaskRepository _repository;

        public GetTaskByIdHandler(ITodoTaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<TodoTask?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id);
        }
    }
}
