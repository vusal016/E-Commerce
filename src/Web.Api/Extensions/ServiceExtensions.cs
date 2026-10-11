namespace Web.Api.Extensions
{
    internal static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserServices>();
            return services;
        }
    }
}