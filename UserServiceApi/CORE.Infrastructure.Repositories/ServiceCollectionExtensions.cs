using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Config;
using CORE.Infrastructure.Repositories.User.Commands;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Repositories.User.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace CORE.Infrastructure.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.ConfigName));


            services.AddScoped<IMainUserCommand, MainUserCommandRepository>();
            services.AddScoped<IMainUserQuery, MainUserQueryRepository>();
        }
    }
}
