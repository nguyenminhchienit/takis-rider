
using CORE.Infrastructure.Repositories.Noti.Interfaces;
using CORE.Infrastructure.Repositories.Noti.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CORE.Infrastructure.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<INoti, NotiRepo>();
        }
    }
}
