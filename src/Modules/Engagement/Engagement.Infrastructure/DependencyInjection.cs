namespace Engagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEngagementModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            services.AddDbContext<EngagementModuleDbContext>(options =>
                options.UseNpgsql(connectionString));
                
            services.AddScoped<IEngagementDbContext>(provider => provider.GetRequiredService<EngagementModuleDbContext>());
            services.AddScoped<DatabaseInitializer>();
            
            services.AddScoped<IEngagementPublicApi, EngagementPublicApi>();
            return services;
        }
    }
}




