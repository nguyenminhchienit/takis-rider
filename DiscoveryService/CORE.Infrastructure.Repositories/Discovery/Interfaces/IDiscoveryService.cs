

using CORE.Infrastructure.Shared.Discovery.Request;

namespace CORE.Infrastructure.Repositories.Discovery.Interfaces
{
    public interface IDiscoveryService
    {
        Task<DiscoveryRequest> GetServiceAsync(string serviceName);
        Task<List<DiscoveryRequest>> GetAllServicesAsync();
    }
}
