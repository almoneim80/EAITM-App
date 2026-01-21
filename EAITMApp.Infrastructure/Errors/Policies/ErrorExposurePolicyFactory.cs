using EAITMApp.SharedKernel.Common;
using EAITMApp.SharedKernel.Errors.Policies;

namespace EAITMApp.Infrastructure.Errors.Policies
{
    /// <summary>
    /// Factory for creating <see cref="IErrorExposurePolicy"/> instances
    /// based on the current application environment.
    /// Ensures that the correct policy is applied for Development, Testing, or Production.
    /// </summary>
    public sealed class ErrorExposurePolicyFactory(AppEnvironment environment)
    {
        /// <inheritdoc/>
        public IErrorExposurePolicy CreateErrorExposurePolicy() 
        {
            return environment switch
            {
                AppEnvironment.Development => new DevelopmentErrorExposurePolicy(),
                AppEnvironment.Testing => new TestingErrorExposurePolicy(),
                AppEnvironment.Production => new ProductionErrorExposurePolicy(),
                _ => new ProductionErrorExposurePolicy() // الأمان أولاً (Default to Prod)
            };
        }
    }
}
