using EAITMApp.Application.Interfaces;
using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.UseCases.Commands.TaskCMD
{
    public record AddTodoTaskCommand(string Title, string Description) : IRequest<TodoTask>, ITransactionalCommand;
}
