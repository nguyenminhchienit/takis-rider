using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.ReviewRide.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.ReviewRide.Commands
{
    public class ReviewRideCommandRequest : ReviewRideRequest, IRequest<ResponseCus<bool>>
    {

        public class CommandHandler : IRequestHandler<ReviewRideCommandRequest, ResponseCus<bool>>
        {

            public readonly IRideCommandRepository rideCommandRepository;

            public CommandHandler(IRideCommandRepository _rideCommandRepository)
            {
                rideCommandRepository = _rideCommandRepository;
            }
            public async Task<ResponseCus<bool>> Handle(ReviewRideCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await rideCommandRepository.SubmitReviewAsync(request);
                    return new ResponseCus<bool>(result);
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(new ResponseCus<bool>(ex.Message));
                }
            }
        }
    }
}
