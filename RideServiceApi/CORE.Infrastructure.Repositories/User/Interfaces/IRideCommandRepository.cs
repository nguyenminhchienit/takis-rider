
using CORE.Applications.Feature.Ride.Model;
using CORE.Infrastructure.Shared.Models.ReviewRide.Request;
using CORE.Infrastructure.Shared.Models.Ride.Request;
using CORE.Infrastructure.Shared.Models.Ride.Response;

namespace CORE.Infrastructure.Repositories.User.Interfaces
{
    public interface IRideCommandRepository
    {
        Task<RideModelResponse> RequestRideAsync(RideModelRequest request);

        Task<bool> UpdateRideStatusAsync(UpdateRideStatusModel request);

        Task<RideModelResponse?> AcceptRideAsync(AcceptRideModel request);

        Task<bool> CancelRideAsync(CancleRideModel request);

        Task<bool> SubmitReviewAsync(ReviewRideRequest review);

    }
}
