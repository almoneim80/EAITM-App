using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.UseCases.Queries
{
    public record GetTaskByIdQuery(Guid Id) : IRequest<TodoTask?>;
}
