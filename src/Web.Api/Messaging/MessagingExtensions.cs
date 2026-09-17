namespace Web.Api.Messaging
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(Program).Assembly
                );  
            });
            services.AddSingleton<InMemoryMessageQueue>();
            services.AddSingleton<IEventBus, EventBus>();
            services.AddHostedService<IntegrationEventProcessorJob>();
            return services;
        }
    }
}