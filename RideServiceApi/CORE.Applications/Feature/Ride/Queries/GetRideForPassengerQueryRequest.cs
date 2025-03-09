using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.Ride.Response;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.Ride.Queries
{
    public class GetRideForPassengerQueryRequest :IRequest<ResponseCus<RideForPassengerModel>>
    {
        public Guid RideId { get; set; }

        public GetRideForPassengerQueryRequest(Guid rideId)
        {
            RideId = rideId;
        }

        public class QueryHandler : IRequestHandler<GetRideForPassengerQueryRequest, ResponseCus<RideForPassengerModel>>
        {
            private readonly IRideQueryRepository rideQueryRepository;

            public QueryHandler(IRideQueryRepository _rideQueryRepository)
            {
                rideQueryRepository = _rideQueryRepository;
            }
            public async Task<ResponseCus<RideForPassengerModel>> Handle(GetRideForPassengerQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await rideQueryRepository.GetRideDetailForPassengerAsync(request.RideId);
                    return new ResponseCus<RideForPassengerModel> (result);
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<RideForPassengerModel>(ex.Message));
                }
            }
        }
    }
}
