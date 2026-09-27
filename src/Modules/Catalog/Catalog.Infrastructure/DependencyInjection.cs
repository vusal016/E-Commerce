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
            services.AddScoped<ICatalogPublicApi, CatalogPublicApi>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCuratedPicksQueryHandler).Assembly));
            services.AddAutoMapper(cfg => cfg.AddProfile<CatalogMapper>());
            return services;
        }
    }
}

