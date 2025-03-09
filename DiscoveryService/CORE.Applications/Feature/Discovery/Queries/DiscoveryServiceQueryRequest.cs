
using CORE.Infrastructure.Repositories.Discovery.Interfaces;
using CORE.Infrastructure.Shared.Discovery.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.Discovery.Queries
{
    public class DiscoveryServiceQueryRequest : IRequest<ResponseCus<DiscoveryRequest>>
    {
        public string ServiceName { get; set; } = string.Empty;
        public DiscoveryServiceQueryRequest(string sv) { 
            ServiceName = sv;
        }

        public class QueryHandler : IRequestHandler<DiscoveryServiceQueryRequest, ResponseCus<DiscoveryRequest>>
        {
            private readonly IDiscoveryService discoveryService;

            public QueryHandler(IDiscoveryService _discoveryService)
            {
                discoveryService = _discoveryService;
            }
            public async Task<ResponseCus<DiscoveryRequest>> Handle(DiscoveryServiceQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await discoveryService.GetServiceAsync(request.ServiceName);
                    return new ResponseCus<DiscoveryRequest>(result);
                }
                catch (Exception ex) { 
                    return await Task.FromResult(new ResponseCus<DiscoveryRequest>(ex.Message));
                }
            }
        }
    }
}
