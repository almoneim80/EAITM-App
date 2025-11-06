using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Queries;
using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.Handlers.TaskHDL
{
    public class GetAllTasksHandler(ITodoTaskRepository repository) : IRequestHandler<GetAllTasksQuery, List<TodoTask>>
    {
        private readonly ITodoTaskRepository _repository = repository;
        public async Task<List<TodoTask>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
