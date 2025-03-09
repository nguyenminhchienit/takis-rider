
using System.Text.Json;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using CORE.Infrastructure.Shared.Models.ReviewRide.Request;
using CORE.Infrastructure.Shared.Models.Ride.Response;
using CORE.Infrastructure.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace CORE.Infrastructure.Repositories.User.Queries
{
    public class RideQueryRepository : IRideQueryRepository
    {

        private readonly DbSqlContext dbSqlContext;
        private readonly HttpClient _httpClient;

        public RideQueryRepository(DbSqlContext _dbSqlContext, HttpClient httpClient)
        {
            dbSqlContext = _dbSqlContext;
            _httpClient = httpClient;
        }
        public async Task<RideModelResponse?> GetRideByIdAsync(Guid rideId)
        {
            var ride = await dbSqlContext.Rides.FindAsync(rideId);
            if (ride == null) return null;

            return new RideModelResponse
            {
                Id = ride.Id,
                PassengerId = ride.PassengerId,
                DriverId = ride.DriverId,
                Status = ride.Status
            };
        }

        public async Task<RideForDriverModel?> GetRideDetailForDriverAsync(Guid rideId)
        {
            var ride = await dbSqlContext.Rides.FindAsync(rideId);
            if (ride == null) return null;

            var passengerInfo = await GetUserInfoAsync(ride.PassengerId);

            return new RideForDriverModel
            {
                Id = ride.Id,
                PassengerInfo = passengerInfo?.Data,
                Status = ride.Status,
                PickupLatitude = ride.PickupLatitude,
                PickupLongitude = ride.PickupLongitude,
                DropoffLatitude = ride.DropoffLatitude,
                DropoffLongitude = ride.DropoffLongitude
            };
        }

        public async Task<RideForPassengerModel?> GetRideDetailForPassengerAsync(Guid rideId)
        {
            var ride = await dbSqlContext.Rides.FindAsync(rideId);
            if (ride == null) return null;

            UserInfoResponse? driverInfo = null;
            if (ride.DriverId != null)
            {
                var driver = await GetUserInfoAsync(ride.DriverId);
                driverInfo = driver?.Data;
            }

            return new RideForPassengerModel
            {
                Id = ride.Id,
                DriverInfo = driverInfo,
                Status = ride.Status,
                PickupLatitude = ride.PickupLatitude,
                PickupLongitude = ride.PickupLongitude,
                DropoffLatitude = ride.DropoffLatitude,
                DropoffLongitude = ride.DropoffLongitude
            };
        }

        public async Task<List<RideModelResponse>> GetRidesByUserAsync(string userId)
        {
            var rides = await dbSqlContext.Rides
                .Where(r => r.PassengerId == userId || r.DriverId == userId)
                .ToListAsync();

            return rides.Select(r => new RideModelResponse
            {
                Id = r.Id,
                PassengerId = r.PassengerId,
                DriverId = r.DriverId,
                Status = r.Status
            }).ToList();
        }

        public async Task<ResponseCus<UserInfoResponse>?> GetUserInfoAsync(string userId) 
        {

            var response = await _httpClient.GetAsync($"https://localhost:7119/user-service/identity/api/User/get-user-by-id/{userId}");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseCus<UserInfoResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }

        public async Task<List<ReviewRideRequest>> GetUserReviewsAsync(string driverId)
        {
            var reviews = await dbSqlContext.RideReviews
                .Where(r => r.TargetUserId == driverId)
                .Select(r => new ReviewRideRequest
                {
                    ReviewerId = r.ReviewerId,
                    TargetUserId = r.TargetUserId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    RideId = r.RideId,
                })
                .ToListAsync();

            return reviews;
        }
    }
}
