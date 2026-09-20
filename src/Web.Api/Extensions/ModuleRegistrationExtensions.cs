namespace Web.Api.Extensions
{
    public static class ModuleRegistrationExtensions
    {
        public static IServiceCollection AddModuleRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityModule(configuration);
            return services;
        }
    }
}