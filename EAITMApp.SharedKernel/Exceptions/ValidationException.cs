using EAITMApp.SharedKernel.Errors.Registries;
using EAITMApp.SharedKernel.Validation;

namespace EAITMApp.SharedKernel.Exceptions
{
    public sealed class ValidationException : BaseAppException
    {
        public IReadOnlyList<ValidationFailure> Failures { get; }

        public ValidationException(
            IReadOnlyList<ValidationFailure> failures
        )
            : base(ValidationErrors.ValidationFailed)
        {
            Failures = failures;
        }
    }
}
