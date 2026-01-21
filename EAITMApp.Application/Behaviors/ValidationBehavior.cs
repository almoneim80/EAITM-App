using EAITMApp.SharedKernel.Validation;
using FluentValidation;
using MediatR;
using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.Application.Behaviors
{
    /// <summary>
    /// MediatR pipeline behavior that enforces request validation using FluentValidation
    /// before invoking the corresponding request handler.
    /// </summary>
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// Executes validation for the incoming request and short-circuits the pipeline
        /// by throwing a validation exception when failures are detected.
        /// </summary>
        /// <param name="request">The incoming request.</param>
        /// <param name="next">Delegate to invoke the next pipeline behavior or handler.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The handler response if validation succeeds.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Are there any rules (Validators) for this request?
            if (!_validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);

            // Convert errors from the external FluentValidation library to our ValidationFailure.
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f != null && !string.IsNullOrWhiteSpace(f.ErrorMessage))
                .Select(f => new ValidationFailure(
                    RuleCode: f.ErrorCode,
                    Message: f.ErrorMessage!,
                    PropertyPath: f.PropertyName!,
                    Severity: DetermineSeverity(f.ErrorCode) // Specify Severity directly
                )).ToList();

            // If at least one error is found, the process is stopped.
            if (failures.Any())
                throw new SharedKernel.Exceptions.ValidationException(failures);

            return await next();
        }

        /// <summary>
        /// Resolves the severity level of a validation failure based on its rule code.
        /// </summary>
        /// <param name="ruleCode">FluentValidation rule code.</param>
        /// <returns>The corresponding <see cref="ErrorSeverity"/>.</returns>
        private static ErrorSeverity DetermineSeverity(string? ruleCode)
        {
            return ruleCode switch
            {
                "NotEmpty" => ErrorSeverity.High,
                "MinLength" => ErrorSeverity.Medium,
                "MaxLength" => ErrorSeverity.Medium,
                "Email" => ErrorSeverity.High,
                _ => ErrorSeverity.Low
            };
        }
    }
}
