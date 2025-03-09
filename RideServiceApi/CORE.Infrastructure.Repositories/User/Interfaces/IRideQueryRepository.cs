using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.Models.ReviewRide.Request;
using CORE.Infrastructure.Shared.Models.Ride.Response;

namespace CORE.Infrastructure.Repositories.User.Interfaces
{
    public interface IRideQueryRepository
    {
        Task<RideModelResponse?> GetRideByIdAsync(Guid rideId);

        Task<List<RideModelResponse>> GetRidesByUserAsync(string userId);

        Task<RideForPassengerModel?> GetRideDetailForPassengerAsync(Guid rideId);
        Task<RideForDriverModel?> GetRideDetailForDriverAsync(Guid rideId);

        // Get Review cua mot tai xe
        Task<List<ReviewRideRequest>> GetUserReviewsAsync(string driverId);
    }
}
