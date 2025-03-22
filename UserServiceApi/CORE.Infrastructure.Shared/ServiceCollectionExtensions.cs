using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using CORE.Infrastructure.Shared.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CORE.Infrastructure.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRegisterSharedServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<DbSqlContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            


            services.AddScoped<UserManager<UserModel>>();
        }


    }
}
