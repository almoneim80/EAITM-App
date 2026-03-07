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

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, 
            InterceptionResult<int> result, 
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if(context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            var now = DateTimeOffset.UtcNow;
            var userId = _currentUser.UserId;

            foreach(var entry in context.ChangeTracker.Entries())
            {
                if(entry.Entity is IAuditableEntity auditableEntity)
                {
                    HandleAudit(entry,  auditableEntity, now, userId);
                }

                if (entry.Entity is ISoftDelete softDelete)
                {
                    HandleSoftDelete(entry, softDelete, now, userId);
                }
                CreateAuditLogIfNeeded(context, entry, now, userId);
            }

            return SavingChangesAsync(eventData, result, cancellationToken);
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
            DateTimeOffset now, 
            string? userId)
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;

                if (entry.Entity is BaseSoftDeletableEntity<Guid> baseEntity)
                    baseEntity.SoftDelete(userId);
            }
        }

        private void CreateAuditLogIfNeeded(
            DbContext context, EntityEntry entry, DateTimeOffset now, string? userId)
        {
            if(entry.State == EntityState.Detached || 
                entry.State == EntityState.Unchanged) return;

            var entityName = entry.Entity.GetType().Name;
            var entityId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue;

            var oldValues = entry.State == EntityState.Modified ? JsonSerializer.Serialize(entry.OriginalValues.ToObject()) : null;
            var newValues = entry.State != EntityState.Deleted ? JsonSerializer.Serialize(entry.CurrentValues.ToObject()) : null;


            var action = entry.State switch
            {
                EntityState.Added => ObjectState.Added,
                EntityState.Modified => ObjectState.Modified,
                EntityState.Deleted => ObjectState.Deleted,
                _ => ObjectState.Unchanged
            };

            var audit = new AuditLog(
                entityName,
                entityId.ToString(),
                action,
                userId ?? "System",
                oldValues,
                newValues,
                _currentUser.IpAddress,
                _currentUser.UserAgent
            );

            context.Set<AuditLog>().Add(audit);
        }
    }
}
