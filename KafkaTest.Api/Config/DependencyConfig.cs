
namespace KafkaTest.Api.Config
{
    public static class DependencyConfig
    {
        public static IServiceCollection AddRegistration(this IServiceCollection services)
        {
            //services.AddSingleton<ISignalRNotificationService, SignalRNotificationService>();
            //services.AddSingleton<IVideoStreamerService, VideoStreamerService>();
            return services;
        }
    }
}
