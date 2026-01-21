using EAITMApp.SharedKernel.Errors.Registries;
using EAITMApp.SharedKernel.Validation;

namespace EAITMApp.SharedKernel.Exceptions
{
    /// <summary>
    /// Represents an exception thrown when one or more validation rules fail.
    /// Encapsulates the collection of <see cref="ValidationFailure"/> instances
    /// that caused the exception.
    /// </summary>
    public sealed class ValidationException : BaseAppException
    {
        public IReadOnlyList<ValidationFailure> Failures { get; }

        public ValidationException(IReadOnlyList<ValidationFailure> failures) : base(ValidationErrors.ValidationFailed)
        {
            Failures = failures;
        }
    }
}
