namespace Ordering.UnitTests.IntegrationTests
{
    public sealed class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private PostgreSqlContainer? _postgres;
        private string _connectionString = string.Empty;
        private string? _fallbackDatabaseName;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((_, config) =>
            {
                var settings = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = _connectionString,
                    ["ConnectionStrings:Database"] = _connectionString,
                    ["ConnectionStrings:Redis"] = "localhost:6379,abortConnect=false",
                    ["Jwt:Secret"] = "K7mQ9xL2vR8pT4zN6wY3cF1hJ5sD8aB0eG4uI7oP2kX9nM6qW3rV5tZ1yH8fC4",
                    ["Jwt:Issuer"] = "E-Commerce",
                    ["Jwt:Audience"] = "E-CommerceClient",
                    ["Jwt:ExpirationInMinutes"] = "15",
                    ["Jwt:RefreshTokenExpiresDays"] = "7"
                };
                config.AddInMemoryCollection(settings);
            });
        }
        public async Task InitializeAsync()
        {
            try
            {
                _postgres = new PostgreSqlBuilder()
                    .WithImage("postgres:16-alpine")
                    .WithDatabase("ecommerce_test")
                    .WithUsername("postgres")
                    .WithPassword("postgres")
                    .Build();
                await _postgres.StartAsync();
                _connectionString = _postgres.GetConnectionString();
            }
            catch
            {
                _postgres = null;
                _fallbackDatabaseName = $"ecommerce_test_{Guid.NewGuid():N}";
                var adminConn = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=connect123";
                var adminOptions = new DbContextOptionsBuilder<OrderingModuleDbContext>().UseNpgsql(adminConn).Options;
                await using (var adminDb = new OrderingModuleDbContext(adminOptions))
                {
                    var createDbSql = string.Concat("CREATE DATABASE \"", _fallbackDatabaseName, "\";");
                    await adminDb.Database.ExecuteSqlRawAsync(createDbSql);
                }
                _connectionString = $"Host=localhost;Port=5432;Database={_fallbackDatabaseName};Username=postgres;Password=connect123";
            }
            Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", _connectionString);
            Environment.SetEnvironmentVariable("ConnectionStrings__Database", _connectionString);
            Environment.SetEnvironmentVariable("ConnectionStrings__Redis", "localhost:6379,abortConnect=false");
            Environment.SetEnvironmentVariable("Jwt__Secret", "K7mQ9xL2vR8pT4zN6wY3cF1hJ5sD8aB0eG4uI7oP2kX9nM6qW3rV5tZ1yH8fC4");
            Environment.SetEnvironmentVariable("Jwt__Issuer", "E-Commerce");
            Environment.SetEnvironmentVariable("Jwt__Audience", "E-CommerceClient");
            Environment.SetEnvironmentVariable("Jwt__ExpirationInMinutes", "15");
            Environment.SetEnvironmentVariable("Jwt__RefreshTokenExpiresDays", "7");
        }
        async Task IAsyncLifetime.DisposeAsync()
        {
            await base.DisposeAsync();
            if (_postgres is not null)
            {
                await _postgres.DisposeAsync();
            }
            else if (!string.IsNullOrWhiteSpace(_fallbackDatabaseName))
            {
                try
                {
                    var adminConn = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=connect123";
                    var adminOptions = new DbContextOptionsBuilder<OrderingModuleDbContext>().UseNpgsql(adminConn).Options;
                    await using var adminDb = new OrderingModuleDbContext(adminOptions);
                    var dropDbSql = string.Concat("DROP DATABASE IF EXISTS \"", _fallbackDatabaseName, "\" WITH (FORCE);");
                    await adminDb.Database.ExecuteSqlRawAsync(dropDbSql);
                }
                catch
                {
                }
            }
        }
    }
}
