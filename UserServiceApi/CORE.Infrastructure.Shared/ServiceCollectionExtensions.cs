using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using CORE.Infrastructure.Shared.Models.User;
using Microsoft.AspNetCore.Identity;
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

            services.AddIdentity<UserModel, IdentityRole>()
            .AddEntityFrameworkStores<DbSqlContext>()
            .AddDefaultTokenProviders();


            services.AddScoped<UserManager<UserModel>>();
        }


    }
}
