
using CORE.Infrastructure.Repositories.User.Commands;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Repositories.User.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CORE.Infrastructure.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<RideQueryRepository>();


            services.AddScoped<IRideQueryRepository, RideQueryRepository>();
            services.AddScoped<IRideCommandRepository, RideCommandRepository>();
        }
    }
}
