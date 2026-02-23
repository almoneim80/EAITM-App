using EAITMApp.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EAITMApp.Infrastructure.Persistence.Interceptors
{
    public sealed class AuditingInterceptor(ICurrentUserService currentUser) : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUser = currentUser;

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, 
            InterceptionResult<int> result, 
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if(context == null) result base.SavedChangesAsync(eventData, result, cancellationToken);
        }
    }
}
