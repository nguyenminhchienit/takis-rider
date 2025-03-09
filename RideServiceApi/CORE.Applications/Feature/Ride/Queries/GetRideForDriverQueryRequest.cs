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
    public class GetRideForDriverQueryRequest : IRequest<ResponseCus<RideForDriverModel>>
    {
        public Guid RideId { get; set; }

        public GetRideForDriverQueryRequest(Guid rideId)
        {
            RideId = rideId;
        }

        public class QueryHandler : IRequestHandler<GetRideForDriverQueryRequest, ResponseCus<RideForDriverModel>>
        {
            private readonly IRideQueryRepository rideQueryRepository;

            public QueryHandler(IRideQueryRepository _rideQueryRepository)
            {
                rideQueryRepository = _rideQueryRepository;
            }
            public async Task<ResponseCus<RideForDriverModel>> Handle(GetRideForDriverQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await rideQueryRepository.GetRideDetailForDriverAsync(request.RideId);
                    return new ResponseCus<RideForDriverModel>(result);
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(new ResponseCus<RideForDriverModel>(ex.Message));
                }
            }
        }
    }
}
