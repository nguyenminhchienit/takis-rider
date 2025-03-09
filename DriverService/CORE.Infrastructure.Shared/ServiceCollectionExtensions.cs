
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CORE.Infrastructure.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRegisterSharedServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<DbSqlContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            
        }


    }
}
