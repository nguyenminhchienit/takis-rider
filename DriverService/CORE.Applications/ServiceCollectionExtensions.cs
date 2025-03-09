using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using CORE.Applications.MessageQueue.Consumer;
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

            services.AddHostedService<DriverConsumer>();

            services.AddHostedService<RideRequestConsumer>();

            services.AddHostedService<LocationNearDriverConsumer>();
        }
       
    }
}



