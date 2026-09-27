namespace Engagement.Infrastructure.Persistence.AutoMig
{
    internal sealed class DatabaseInitializer(EngagementModuleDbContext dbContext)
    {
        public Task InitializeAsync(CancellationToken cancellationToken = default)
            => dbContext.Database.MigrateAsync(cancellationToken);
    }

    public static class DatabaseMigrationExtensions
    {
        public static async Task MigrateEngagementDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
            await initializer.InitializeAsync();
        }
    }
}
