namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOrderingModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<OrderingModuleDbContext>((sp, options) =>
            {
                options.UseNpgsql(connectionString);
                var interceptor = sp.GetServices<ISaveChangesInterceptor>();
                options.AddInterceptors(interceptor);
            });
            services.AddScoped<IOrderingDbContext>(provider => provider.GetRequiredService<OrderingModuleDbContext>());
            services.AddScoped<DatabaseInitializer>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(PlaceOrderCommand).Assembly));
            return services;
        }
    }
}