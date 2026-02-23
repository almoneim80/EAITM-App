using EAITMApp.Application.Interfaces;
using EAITMApp.Application.Persistence;
using MediatR;

namespace EAITMApp.Infrastructure.Behaviors
{
    /// <summary>
    /// MediatR Pipeline Behavior that encapsulates the request within a Unit of Work transaction.
    /// Only triggers for requests implementing ITransactionalCommand.
    /// </summary>
    public class TransactionBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, ITransactionalCommand
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ExecuteAsync(async ct =>
            {
                return await next();
            }, cancellationToken);
        }
    }
}
