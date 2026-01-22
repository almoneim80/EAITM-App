using EAITMApp.SharedKernel.Errors;

namespace EAITMApp.SharedKernel.Exceptions
{
    public sealed class NotFoundException(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null)
            : BaseAppException(descriptor, metadata);

    public sealed class ConflictException(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null)
        : BaseAppException(descriptor, metadata);

    public sealed class ForbiddenException(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null)
        : BaseAppException(descriptor, metadata);

    public sealed class BadRequestException(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null)
        : BaseAppException(descriptor, metadata);

    public sealed class UnauthorizedException(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null)
        : BaseAppException(descriptor, metadata);

    public sealed class InvalidRequestException(ErrorDescriptor descriptor,  IDictionary<string, object>? metadata = null)
        : BaseAppException(descriptor, metadata);
}
