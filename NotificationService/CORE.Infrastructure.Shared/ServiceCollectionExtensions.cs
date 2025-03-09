
using CORE.Infrastructure.Shared.ConfigDB.MongoDB;
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CORE.Infrastructure.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRegisterSharedServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<DbSqlContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<NotificationDbContext>();

            //var serviceProvider = services.BuildServiceProvider();
            //var mongoDbContext = serviceProvider.GetRequiredService<NotificationDbContext>();
            //mongoDbContext.SeedData();

        }


        

    }
}
