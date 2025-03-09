using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Discovery.Interfaces;
using CORE.Infrastructure.Shared.Discovery.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.Discovery.Queries
{
    public class DiscoveryServiceGetAllQueryRequest : IRequest<ResponseCus<List<DiscoveryRequest>>>
    {
        public class QueryHandler : IRequestHandler<DiscoveryServiceGetAllQueryRequest, ResponseCus<List<DiscoveryRequest>>>
        {

            private readonly IDiscoveryService discoveryService;

            public QueryHandler(IDiscoveryService _discoveryService)
            {
                discoveryService = _discoveryService;
            }
            public async Task<ResponseCus<List<DiscoveryRequest>>> Handle(DiscoveryServiceGetAllQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await discoveryService.GetAllServicesAsync();
                    return new ResponseCus<List<DiscoveryRequest>>(result); 
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(new ResponseCus<List<DiscoveryRequest>>(ex.Message));
                }
            }
        }
    }
}
