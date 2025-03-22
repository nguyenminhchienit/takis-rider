using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using CORE.Applications.Feature.Notification.MessageConsumer;
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

            //services.AddHostedService<NotificationConsumer>();

            services.AddHostedService<NotificationFromDriveConsumer>();
        }
       
    }
}



