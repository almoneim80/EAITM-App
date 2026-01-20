namespace EAITMApp.SharedKernel.Errors.Enums
{
    /// <summary>
    /// Defines a restricted set of HTTP status codes used by the application
    /// to map domain and application errors to API responses in a controlled manner.
    /// </summary>
    public enum AppHttpStatus
    {
        Ok = 200,                  // Successfully
        BadRequest = 400,          // Client-side input errors
        Unauthorized = 401,        // Authentication is required
        Forbidden = 403,           // Client does not have permissions
        NotFound = 404,            // Resource could not be found
        Conflict = 409,            // Conflict 
        InternalServerError = 500  // Server-side error
    }
}
