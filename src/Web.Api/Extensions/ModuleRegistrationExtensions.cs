namespace Web.Api.Extensions
{
    public static class ModuleRegistrationExtensions
    {
        public static IServiceCollection AddModuleRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityModule(configuration);
            services.AddCatalogModule(configuration);
            services.AddPromotionsModule(configuration);

            return services;
        }
    }
}