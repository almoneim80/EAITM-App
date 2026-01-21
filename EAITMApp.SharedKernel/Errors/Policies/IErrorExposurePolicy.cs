using EAITMApp.SharedKernel.Errors.Contracts;

namespace EAITMApp.SharedKernel.Errors.Policies
{
    /// <summary>
    /// Defines a contract for applying error exposure rules.
    /// Responsible for adjusting or filtering <see cref="ApiError"/> information
    /// before it is returned to the client based on the execution context and exception.
    /// </summary>
    public interface IErrorExposurePolicy
    {
        /// <summary>
        /// Applies exposure rules to the provided <see cref="ApiError"/>.
        /// May redact or modify the error details depending on environment, sensitivity, or exception type.
        /// </summary>
        ApiError Apply(ApiError error, ErrorContext context, Exception exception);
    }
}
