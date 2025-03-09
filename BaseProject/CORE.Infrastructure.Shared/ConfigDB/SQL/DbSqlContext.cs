﻿
using Microsoft.EntityFrameworkCore;

namespace CORE.Infrastructure.Shared.ConfigDB.SQL
{
    public class DbSqlContext : DbContext
    {
        public DbSqlContext(DbContextOptions<DbSqlContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


    }
}
