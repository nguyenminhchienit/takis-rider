using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.ReviewRide.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.ReviewRide.Queries
{
    public class GetReviewRideOfDriverQueryRequest :IRequest<ResponseCus<List<ReviewRideRequest>>>
    {
        public string driverId { get; set; } = string.Empty;

        public GetReviewRideOfDriverQueryRequest(string _driverId)
        {
            driverId = _driverId;
        }

        public class QueryHandler : IRequestHandler<GetReviewRideOfDriverQueryRequest, ResponseCus<List<ReviewRideRequest>>>
        {
            private readonly IRideQueryRepository repository;

            public QueryHandler(IRideQueryRepository rideQueryRepository)
            {
                repository = rideQueryRepository;
            }
            public async Task<ResponseCus<List<ReviewRideRequest>>> Handle(GetReviewRideOfDriverQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await repository.GetUserReviewsAsync(request.driverId);
                    return new ResponseCus<List<ReviewRideRequest>>(result);
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<List<ReviewRideRequest>>(ex.Message));
                }

            }
        }
    }
}
