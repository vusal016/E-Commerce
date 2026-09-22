namespace Web.Api.Caching
{
    public static class CachingExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Redis");

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(QueryCachingPipelineBehavior<,>));
            return services;
        }
    }
}