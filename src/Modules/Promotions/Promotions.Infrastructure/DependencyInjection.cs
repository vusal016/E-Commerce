namespace Promotions.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPromotionsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PromotionsModuleDbContext>((sp, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                var interceptor = sp.GetServices<ISaveChangesInterceptor>();
                options.AddInterceptors(interceptor);
            });

            services.AddScoped<IPromotionsDbContext>(provider => provider.GetRequiredService<PromotionsModuleDbContext>());
            services.AddScoped<DatabaseInitializer>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetHeroBannersQueryHandler).Assembly));
            services.AddAutoMapper(cfg => cfg.AddProfile<PromotionsMapper>());

            return services;
        }
    }
}