
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.Ride.Response;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.Ride.Queries
{
    public class GetRideByIdQueryRequest : IRequest<ResponseCus<RideModelResponse>>
    {
        public Guid Id { get; set; }

        public GetRideByIdQueryRequest(Guid rideId)
        {
            Id = rideId;
        }

        public class QueryHandler : IRequestHandler<GetRideByIdQueryRequest, ResponseCus<RideModelResponse>>
        {

            private readonly IRideQueryRepository rideQueryRepository;

            public QueryHandler(IRideQueryRepository _rideQueryRepository)
            {
                rideQueryRepository = _rideQueryRepository;
            }
            public async Task<ResponseCus<RideModelResponse>> Handle(GetRideByIdQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await rideQueryRepository.GetRideByIdAsync(request.Id);
                    return new ResponseCus<RideModelResponse>(result);
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<RideModelResponse>(ex.Message));
                }
            }
        }
    }
}
