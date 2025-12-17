using EAITMApp.Application.Common.Enums;
using EAITMApp.Application.Common.Responses;
using EAITMApp.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace EAITMApp.Infrastructure.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        // نحقن قائمة بجميع الـ Validators المسجلة لهذا النوع من الطلب (TRequest)
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // التحقق مما إذا كان هناك أي Validators مسجل لهذا الـ Request
            if (_validators.Any())
            {
                // تنفيذ جميع عمليات التحقق بشكل متوازٍ
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                // تجميع جميع الأخطاء من جميع الـ Validators
                var failures = validationResults.Where(r => r.Errors.Any()).SelectMany(r => r.Errors).ToList();

                // إذا وُجدت أي أخطاء، قم برمي استثناء ValidationException
                if (failures.Any())
                {
                    var mappedErrors = failures.Select(f => 
                    new ApiError(
                        Code: "VALIDATION_ERROR", 
                        Message: f.ErrorMessage, 
                        Property: f.PropertyName,
                        Severity: ErrorSeverity.Low)).ToList();
                    throw new RequestValidationException(mappedErrors);
                }
            }

            // إذا لم تكن هناك أخطاء، انتقل إلى الـ Behavior التالي أو الـ Handler النهائي.
            return await next(cancellationToken);
        }
    }
}
