namespace Web.Api.DatabaseMigrations
{
    internal static class DatabaseMigrationExtensions
    {
        public static async Task MigrateAllDatabasesAsync(this IServiceProvider services)
        {
            await services.MigrateIdentityDatabaseAsync();
            await services.MigrateCatalogDatabaseAsync();
            await services.MigratePromotionsDatabaseAsync();
            await services.MigrateCartDatabaseAsync();
            await services.MigrateEngagementDatabaseAsync();
            await services.MigrateOrderingDatabaseAsync();
        }
    }
}