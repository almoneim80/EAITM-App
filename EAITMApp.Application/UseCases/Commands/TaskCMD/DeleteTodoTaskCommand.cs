using EAITMApp.Application.Interfaces;
using MediatR;

namespace EAITMApp.Application.UseCases.Commands.TaskCMD
{
    public record DeleteTodoTaskCommand(Guid Id) : IRequest<bool>, ITransactionalCommand;
}
