namespace Web.Api.DatabaseMigrations
{
    public static class DatabaseMigrationExtensions
    {
        public static async Task MigrateAllDatabasesAsync(this IServiceProvider services)
        {
            await services.MigrateIdentityDatabaseAsync();
        }
    }
}