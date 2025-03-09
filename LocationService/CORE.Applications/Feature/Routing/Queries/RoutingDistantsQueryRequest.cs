using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Routing.Interface;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.Routing.Queries
{
    public class RoutingDistantsQueryRequest : IRequest<ResponseCus<string>>
    {
        
        public double startLat {  get; set; }

        public double startLng { get; set; }

        public double endLat { get; set; }

        public double endLng { get; set; }

        public RoutingDistantsQueryRequest(double _startLat, double _startLng, double _endLat, double _endLng)
        {
            startLat = _startLat;
            startLng = _startLng;
            endLat = _endLat;
            endLng = _endLng;
        }

        public class QueryHandler : IRequestHandler<RoutingDistantsQueryRequest, ResponseCus<string>>
        {
            private readonly IRoutingService _routingService;
            public QueryHandler(IRoutingService routingService)
            {
                _routingService = routingService;
            }
            public async Task<ResponseCus<string>> Handle(RoutingDistantsQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _routingService.GetRouteAsync(request.startLat, request.startLng, request.endLat, request.endLng);
                    return new ResponseCus<string>(result);
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<string>(ex.Message));
                }
            }
        }

    }
}
