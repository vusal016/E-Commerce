namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOrderingModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            services.AddDbContext<OrderingModuleDbContext>(options =>
                options.UseNpgsql(connectionString));
                
            services.AddScoped<IOrderingDbContext>(provider => provider.GetRequiredService<OrderingModuleDbContext>());
            services.AddScoped<DatabaseInitializer>();
            
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Ordering.Application.Features.PlaceOrder.PlaceOrderCommand).Assembly));
            
            return services;
        }
    }
}


