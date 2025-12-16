using EAITMApp.Application.Common.Responses;

namespace EAITMApp.Application.Exceptions
{
    public class RequestValidationException :  Exception
    {
        public IReadOnlyList<ApiError> Errors { get; }
        public RequestValidationException(IEnumerable<ApiError> errors)
            : base("One or more validation failures have occurred.")
        {
            Errors = errors.ToList();
        }
    }
}
