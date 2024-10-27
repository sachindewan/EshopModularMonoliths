using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.DDD;

namespace Shared.Data.Interceptors
{
    public class AuditableInterceptor : SaveChangesInterceptor
    {
        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            UpdateEntities(eventData.Context);
            return base.SavedChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            var now = DateTime.UtcNow;
            var entries = context.ChangeTracker.Entries<IEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = "system"; // TODO: get current user
                }
                else if (entry.State == EntityState.Modified || entry.HasChangedOwnProperty())
                {
                    entry.Entity.LastModified = now;
                    entry.Entity.LastModifiedBy = "system"; // TODO: get current user
                }
            }
        }
    }
    public static class Extensions
    {
        public static bool HasChangedOwnProperty(this EntityEntry entry)
        {
            return entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                r.TargetEntry.State == EntityState.Modified);
        }
    }
}
