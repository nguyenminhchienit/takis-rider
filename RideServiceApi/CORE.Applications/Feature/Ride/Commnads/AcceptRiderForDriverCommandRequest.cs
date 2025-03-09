
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.Ride.Request;
using CORE.Infrastructure.Shared.Models.Ride.Response;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.Ride.Commnads
{
    public class AcceptRiderForDriverCommandRequest : AcceptRideModel, IRequest<ResponseCus<RideModelResponse>>
    {

        public class QueryHanlder : IRequestHandler<AcceptRiderForDriverCommandRequest, ResponseCus<RideModelResponse>>
        {
            private readonly IRideCommandRepository rideCommandRepository;

            public QueryHanlder(IRideCommandRepository _rideCommandRepository)
            {
                rideCommandRepository = _rideCommandRepository;
            }
            public async Task<ResponseCus<RideModelResponse>> Handle(AcceptRiderForDriverCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await rideCommandRepository.AcceptRideAsync(request);
                    return new ResponseCus<RideModelResponse>(result);
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<RideModelResponse>(ex.Message));
                }
            }
        }
    }
}
