
using CORE.Applications.MessageQueue.Ride;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.Ride.Request;
using CORE.Infrastructure.Shared.Models.Ride.Response;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.Ride.Commnads
{
    public class RequestRideCommandRequest : RideModelRequest, IRequest<ResponseCus<RideModelResponse>>
    {

        public class QueryHandler : IRequestHandler<RequestRideCommandRequest, ResponseCus<RideModelResponse>>
        {

            private readonly IRideCommandRepository rideCommandRepository;
            private readonly RideProducer rideProducer;

            public QueryHandler(IRideCommandRepository _rideCommandRepository, RideProducer _rideProducer)
            {
                rideCommandRepository = _rideCommandRepository;
                rideProducer = _rideProducer;
            }
            public async Task<ResponseCus<RideModelResponse>> Handle(RequestRideCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {

                    var result = await rideCommandRepository.RequestRideAsync(request);
                    rideProducer.SendNotification("Bạn đã gửi yêu cầu đặt chuyến thành công");
                    var rideallocation = new RideAllocationRequest
                    {
                        RideId = result.Id,
                        UserId = result.PassengerId,
                        PickupLatitude = request.PickupLatitude,
                        PickupLongitude = request.PickupLongitude
                    };
                    rideProducer.SendRideRequest(rideallocation);
                    return new ResponseCus<RideModelResponse>(result);
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<RideModelResponse>(ex.Message));
                }
            }
        }
    }
}
