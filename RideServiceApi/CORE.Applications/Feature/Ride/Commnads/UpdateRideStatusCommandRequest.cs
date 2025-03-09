
using CORE.Applications.Feature.Ride.Model;
using CORE.Applications.MessageQueue.Ride;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.Ride.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.Ride.Commnads
{
    public class UpdateRideStatusCommandRequest : UpdateRideStatusModel, IRequest<ResponseCus<bool>>
    {
        public class QueryHandler : IRequestHandler<UpdateRideStatusCommandRequest, ResponseCus<bool>>
        {
            private readonly IRideCommandRepository rideCommandRepository;
            private readonly RideEventPublisher _rideEventPublisher;

            public QueryHandler(IRideCommandRepository _rideCommandRepository)
            {
                rideCommandRepository = _rideCommandRepository; 
            }
            public async Task<ResponseCus<bool>> Handle(UpdateRideStatusCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request.Status == RideStatus.Arrived)
                    {
                        _rideEventPublisher.PublishDriverArrivedEvent(request.Id);
                    }
                    var result = await rideCommandRepository.UpdateRideStatusAsync(request);
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
