using Catalog.Infrastructure.Persistence.AutoMig;

namespace Catalog.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogModuleDbContext>((sp, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                var interceptor = sp.GetServices<ISaveChangesInterceptor>();
                options.AddInterceptors(interceptor);
            });


            services.AddScoped<ICatalogDbContext>(provider => provider.GetRequiredService<CatalogModuleDbContext>());
            services.AddScoped<DatabaseIntializer>();
            //MediaTR
            return services;
        }
    }
}