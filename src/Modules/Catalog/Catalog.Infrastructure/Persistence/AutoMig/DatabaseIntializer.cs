namespace Catalog.Infrastructure.Persistence.AutoMig
{
    internal sealed class DatabaseIntializer(CatalogModuleDbContext dbContext)
    {
        public Task InitializeAsync(CancellationToken cancellationToken = default)
        => dbContext.Database.MigrateAsync(cancellationToken);
    }

    public static class DatabaseIntializerExtensions
    {
        public static async Task MigrateCatalogDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<DatabaseIntializer>();
            await initializer.InitializeAsync();
        }
    }
}