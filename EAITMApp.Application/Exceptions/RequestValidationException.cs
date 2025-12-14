namespace EAITMApp.Application.Exceptions
{
    public class RequestValidationException :  Exception
    {
        public IReadOnlyList<ValidationError> Errors { get; }
        public RequestValidationException(IEnumerable<ValidationError> errors)
            : base("One or more validation failures have occurred.")
        {
            Errors = errors.ToList();
        }
    }
}
