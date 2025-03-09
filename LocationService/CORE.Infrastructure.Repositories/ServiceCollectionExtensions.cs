
using CORE.Infrastructure.Repositories.Location.Commands;
using CORE.Infrastructure.Repositories.Location.Interfaces;
using CORE.Infrastructure.Repositories.Location.Queries;
using CORE.Infrastructure.Repositories.Routing.Interface;
using CORE.Infrastructure.Repositories.Routing.Query;
using Microsoft.AspNetCore.Builder;
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
            services.AddHttpClient();

            services.AddScoped<IRoutingService, RoutingService>();
            services.AddScoped<ILocationCommandRepository, LocationCommandRepository>();
            services.AddScoped<ILocationQueryRepository, LocationQueryRepository>();

        }
    }
}
