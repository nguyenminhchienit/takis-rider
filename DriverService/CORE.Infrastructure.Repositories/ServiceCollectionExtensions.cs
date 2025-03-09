
using CORE.Infrastructure.Repositories.Driver.Commands;
using CORE.Infrastructure.Repositories.Driver.Interfaces;
using CORE.Infrastructure.Repositories.Driver.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;


namespace CORE.Infrastructure.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                var redisConfig = configuration["Redis:Host"] + ":" + configuration["Redis:Port"];
                return ConnectionMultiplexer.Connect(redisConfig);
            });

            services.AddScoped<IDriverCommandRepository, DriverCommandRepository>();

            services.AddSingleton<LocationRequestProducer>();
            services.AddSingleton<DriverRequestProducer>();
            services.AddSingleton<NotificationProducer>();
        }
    }
}
