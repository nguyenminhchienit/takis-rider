using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CORE.Infrastructure.Shared.ConfigDB.SQL
{
    public class DbSqlContext : IdentityDbContext<UserModel>
    {
        public DbSqlContext(DbContextOptions<DbSqlContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
