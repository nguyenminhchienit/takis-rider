
using System.Reflection;
using CORE.Applications.Consumer;
using CORE.Applications.Feature.Location.BackgroundJob;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CORE.Applications
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddHostedService<LocationConsumer>();
            services.AddHostedService<DriverLocationUpdater>();
        }
       
    }
}



