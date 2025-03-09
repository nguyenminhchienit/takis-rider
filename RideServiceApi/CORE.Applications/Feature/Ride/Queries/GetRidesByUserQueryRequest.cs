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
    public class GetRidesByUserQueryRequest : IRequest<ResponseCus<List<RideModelResponse>>>
    {
        public string UserId { get; set; } = string.Empty;

        public GetRidesByUserQueryRequest(string userId)
        {
            UserId = userId;    
        }

        public class QueryHandler : IRequestHandler<GetRidesByUserQueryRequest, ResponseCus<List<RideModelResponse>>>
        {

            private readonly IRideQueryRepository rideQueryRepository;

            public QueryHandler(IRideQueryRepository _rideQueryRepository)
            {
                rideQueryRepository = _rideQueryRepository;
            }
            public async Task<ResponseCus<List<RideModelResponse>>> Handle(GetRidesByUserQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await rideQueryRepository.GetRidesByUserAsync(request.UserId);
                    return new ResponseCus<List<RideModelResponse>>(result);
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<List<RideModelResponse>>(ex.Message));
                }
            }
        }
    }
}
