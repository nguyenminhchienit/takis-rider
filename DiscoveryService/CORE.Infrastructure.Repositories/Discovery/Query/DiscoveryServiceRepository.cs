using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using CORE.Infrastructure.Repositories.Discovery.Interfaces;
using CORE.Infrastructure.Shared.Discovery.Request;

namespace CORE.Infrastructure.Repositories.Discovery.Query
{
    public class DiscoveryServiceRepository : IDiscoveryService
    {
        private readonly IConsulClient _consulClient;

        public DiscoveryServiceRepository(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        public async Task<DiscoveryRequest?> GetServiceAsync(string serviceName)
        {
            var services = await _consulClient.Agent.Services();
            var service = services.Response.Values.FirstOrDefault(s => s.Service == serviceName);

            if (service == null) return null;

            return new DiscoveryRequest
            {
                Name = service.Service,
                Address = service.Address,
                Port = service.Port
            };
        }

        public async Task<List<DiscoveryRequest>> GetAllServicesAsync()
        {
            var services = await _consulClient.Agent.Services();
            return services.Response.Values.Select(s => new DiscoveryRequest
            {
                Name = s.Service,
                Address = s.Address,
                Port = s.Port
            }).ToList();
        }
    }
}
