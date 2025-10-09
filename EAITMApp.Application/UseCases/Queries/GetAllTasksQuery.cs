using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.UseCases.Queries
{
    public record GetAllTasksQuery() : IRequest<List<TodoTask>>;
}
