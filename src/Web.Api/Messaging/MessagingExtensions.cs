namespace Web.Api.Messaging
{
    internal static class MessagingExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(Program).Assembly,
                    typeof(PlaceOrderCommand).Assembly,
                    typeof(AddItemCommand).Assembly
                );
            });
            services.AddSingleton<InMemoryMessageQueue>();
            services.AddSingleton<IEventBus, EventBus>();
            services.AddHostedService<IntegrationEventProcessorJob>();
            return services;
        }
    }
}