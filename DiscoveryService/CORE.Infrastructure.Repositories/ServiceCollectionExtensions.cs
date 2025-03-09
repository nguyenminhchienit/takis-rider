
using Consul;
using CORE.Infrastructure.Repositories.Discovery.Interfaces;
using CORE.Infrastructure.Repositories.Discovery.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace CORE.Infrastructure.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDiscoveryService,DiscoveryServiceRepository>();


        }
    }
}
