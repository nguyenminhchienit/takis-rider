
using CORE.Applications.Feature.Ride.Model;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using CORE.Infrastructure.Shared.Models.ReviewRide.Request;
using CORE.Infrastructure.Shared.Models.Ride.Request;
using CORE.Infrastructure.Shared.Models.Ride.Response;
using Microsoft.EntityFrameworkCore;



namespace CORE.Infrastructure.Repositories.User.Commands
{
    public class RideCommandRepository : IRideCommandRepository
    {
        private readonly DbSqlContext dbSqlContext;
        

        public RideCommandRepository(DbSqlContext _dbSqlContext)
        {
            dbSqlContext = _dbSqlContext;
        }

        public async Task<RideModelResponse?> AcceptRideAsync(AcceptRideModel request)
        {
            var ride = await dbSqlContext.Rides.FindAsync(request.Id);
            if (ride == null || ride.Status != RideStatus.Pending) return null;

            ride.DriverId = request.DriverId;
            ride.Status = RideStatus.Accepted;
            await dbSqlContext.SaveChangesAsync();

            return new RideModelResponse
            {
                Id = ride.Id,
                PassengerId = ride.PassengerId,
                DriverId = ride.DriverId,
                Status = ride.Status
            };
        }

        public async Task<bool> CancelRideAsync(CancleRideModel request)
        {
            var ride = await dbSqlContext.Rides.FindAsync(request.Id);
            if (ride == null) return false;

            if (ride.PassengerId != request.UserId && ride.DriverId != request.UserId)
                return false; // Không phải chủ chuyến đi

            if (ride.Status == RideStatus.Completed || ride.Status == RideStatus.InProgress)
                return false; // Không thể hủy

            ride.Status = RideStatus.Canceled;
            await dbSqlContext.SaveChangesAsync();
            return true;
        }

        public async Task<RideModelResponse> RequestRideAsync(RideModelRequest request)
        {
            var ride = new RideModel
            {
                Id = Guid.NewGuid(),
                PassengerId = request.PassengerId,
                PickupLatitude = request.PickupLatitude,
                PickupLongitude = request.PickupLongitude,
                DropoffLatitude = request.DropoffLatitude,
                DropoffLongitude = request.DropoffLongitude,
                Status = RideStatus.Pending
            };

            await dbSqlContext.Rides.AddAsync(ride);
            await dbSqlContext.SaveChangesAsync();

            return new RideModelResponse
            {
                Id = ride.Id,
                PassengerId = ride.PassengerId,
                Status = ride.Status
            };
        }

        public async Task<bool> SubmitReviewAsync(ReviewRideRequest review)
        {
            var reviewModel = new RideReviewModel
            {
                Id = Guid.NewGuid(),
                RideId = review.RideId,
                ReviewerId = review.ReviewerId,
                TargetUserId = review.TargetUserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await dbSqlContext.RideReviews.AddAsync(reviewModel);
            await dbSqlContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRideStatusAsync(UpdateRideStatusModel request)
        {
            var ride = await dbSqlContext.Rides.FindAsync(request.Id);
            if (ride == null) return false;
            
            ride.Status = request.Status;
            await dbSqlContext.SaveChangesAsync();
            return true;
        }
    }
}
