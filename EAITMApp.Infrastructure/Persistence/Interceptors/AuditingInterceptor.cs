using EAITMApp.Application.Interfaces;
using EAITMApp.Domain.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using EAITMApp.Domain.Enums;
using EAITMApp.Domain.Logs;

namespace EAITMApp.Infrastructure.Persistence.Interceptors
{
    public sealed class AuditingInterceptor(ICurrentUserService currentUser) : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUser = currentUser;

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, 
            InterceptionResult<int> result, 
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if(context == null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var now = DateTimeOffset.UtcNow;
            var userId = _currentUser.UserId ?? "System";
            var auditLogs = new List<AuditLog>();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if(entry.Entity is IAuditableEntity auditableEntity)
                {
                    HandleAudit(entry,  auditableEntity, now, userId);
                }

                if (entry.Entity is ISoftDelete softDelete)
                {
                    HandleSoftDelete(entry, softDelete, userId);
                }

                if (entry.Entity.GetType() != typeof(AuditLog) && 
                    entry.State is EntityState.Added or 
                    EntityState.Modified or 
                    EntityState.Deleted)
                {
                    var audit = CreateAuditEntry(entry, userId);
                    if (audit != null) auditLogs.Add(audit);
                }
            }

            if (auditLogs.Any())
            {
                await context.Set<AuditLog>().AddRangeAsync(auditLogs, cancellationToken);
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void HandleAudit(
            EntityEntry entry, 
            IAuditableEntity auditable,
            DateTimeOffset now, 
            string? userId)
        {
            if (entry.State == EntityState.Added)
                auditable.SetCreation(now, userId);

            if (entry.State == EntityState.Modified)
                auditable.SetModification(now, userId);
        }

        private void HandleSoftDelete(
            EntityEntry entry, 
            ISoftDelete softDelete,
            string? userId)
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;

                softDelete.SoftDelete(userId);
            }
        }

        private AuditLog? CreateAuditEntry(EntityEntry entry, string userId)
        {
            var entityName = entry.Entity.GetType().Name;
            var idProperty = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
            var entityId = idProperty?.CurrentValue?.ToString() ?? "Temporary/New";


            var action = entry.State switch
            {
                EntityState.Added => ObjectState.Added,
                EntityState.Modified => ObjectState.Modified,
                EntityState.Deleted => ObjectState.Deleted,
                _ => ObjectState.Unchanged
            };

            return new AuditLog(
                entityName,
                entityId.ToString(),
                action,
                userId,
                entry.State == EntityState.Modified ? JsonSerializer.Serialize(entry.OriginalValues.ToObject()) : null,
                entry.State != EntityState.Deleted ? JsonSerializer.Serialize(entry.CurrentValues.ToObject()) : null,
                _currentUser.IpAddress,
                _currentUser.UserAgent
            );
        }
    }
}
