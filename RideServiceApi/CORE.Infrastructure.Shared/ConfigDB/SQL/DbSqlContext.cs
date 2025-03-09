
using CORE.Infrastructure.Shared.Models.ReviewRide.Request;
using CORE.Infrastructure.Shared.Models.Ride.Request;
using Microsoft.EntityFrameworkCore;

namespace CORE.Infrastructure.Shared.ConfigDB.SQL
{
    public class DbSqlContext : DbContext
    {
        public DbSqlContext(DbContextOptions<DbSqlContext> options) : base(options) { }

        public DbSet<RideModel> Rides { get; set; }
        public DbSet<RideReviewModel> RideReviews { get; set; }


    }
}
