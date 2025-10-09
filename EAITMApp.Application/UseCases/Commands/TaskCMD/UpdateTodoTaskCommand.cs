using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.UseCases.Commands.TaskCMD
{
    public record UpdateTodoTaskCommand(Guid Id, string Title, string Description, bool IsCompleted) : IRequest<TodoTask?>;
}
