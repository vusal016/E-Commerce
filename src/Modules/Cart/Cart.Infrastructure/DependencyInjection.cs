namespace Cart.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCartModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CartModuleDbContext>((sp, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                var interceptor = sp.GetServices<ISaveChangesInterceptor>();
                options.AddInterceptors(interceptor);
            });

            services.AddScoped<ICartDbContext>(provider => provider.GetRequiredService<CartModuleDbContext>());
            services.AddScoped<DatabaseInitializer>();
            
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CartMapper).Assembly));
            services.AddAutoMapper(cfg => cfg.AddProfile<CartMapper>());

            return services;
        }
    }
}