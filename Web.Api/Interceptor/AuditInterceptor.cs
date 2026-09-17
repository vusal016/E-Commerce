namespace Web.Api.Interceptor
{
    public sealed class AuditInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            var entities = eventData.Context.ChangeTracker.Entries();

            foreach (var entry in entities)
            {

                if (entry.Entity is IAuiditEntity audit)
                {
                    if (entry.State == EntityState.Added)
                        audit.CreatedAt = DateTime.UtcNow;

                    if (entry.State == EntityState.Modified)
                        audit.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}