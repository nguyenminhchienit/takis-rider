
using CORE.Infrastructure.Repositories.Location.Interfaces;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.Location.Queries
{
    public class FindNearestDriverQueryRequest : IRequest<ResponseCus<IEnumerable<string>>>
    {
        public double latitude { get; set; }

        public double longitude { get; set; }

        public double radius { get; set; }

        public FindNearestDriverQueryRequest(double _latitude, double _longitude, double _radiusKm)
        {
            latitude = _latitude;
            longitude = _longitude; 
            radius = _radiusKm;
        }

        public class QueryHandler : IRequestHandler<FindNearestDriverQueryRequest, ResponseCus<IEnumerable<string>>>
        {

            private readonly ILocationQueryRepository repository;


            public QueryHandler(ILocationQueryRepository _repository)
            {
                repository = _repository;
            }
            public async Task<ResponseCus<IEnumerable<string>>> Handle(FindNearestDriverQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await repository.FindNearestDriversAsync(request.latitude, request.longitude, request.radius);
                    return new ResponseCus<IEnumerable<string>>(result);
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(new ResponseCus<IEnumerable<string>>(ex.Message));
                }
            }
        }
    }
}
