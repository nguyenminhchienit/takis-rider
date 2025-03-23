using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Config;
using CORE.Infrastructure.Repositories.Services.Authen;
using CORE.Infrastructure.Repositories.Services.EmailService;
using CORE.Infrastructure.Repositories.User.Commands;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Repositories.User.Queries;
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using CORE.Infrastructure.Shared.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
            services.AddScoped<Authenticate>();
            services.AddScoped<EmailService>();
            services.AddScoped<SmsService>();

            services.AddHttpContextAccessor();
        }
    }
}
