
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.Ride.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.Ride.Commnads
{
    public class CancleRideCommandRequest : CancleRideModel, IRequest<ResponseCus<bool>>
    {
        public class QueryHandler : IRequestHandler<CancleRideCommandRequest, ResponseCus<bool>>
        {

            private readonly IRideCommandRepository rideCommandRepository;

            public QueryHandler(IRideCommandRepository _rideCommandRepository)
            {
                rideCommandRepository = _rideCommandRepository;
            }
            public async Task<ResponseCus<bool>> Handle(CancleRideCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await rideCommandRepository.CancelRideAsync(request);
                    return new ResponseCus<bool> ( result );
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<bool>(ex.Message));
                }
            }
        }
    }
}
