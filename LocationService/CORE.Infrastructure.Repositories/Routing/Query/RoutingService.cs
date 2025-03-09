using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Routing.Interface;
using Microsoft.Extensions.Configuration;

namespace CORE.Infrastructure.Repositories.Routing.Query
{
    public class RoutingService : IRoutingService
    {

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public RoutingService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task<string> GetRouteAsync(double startLat, double startLng, double endLat, double endLng)
        {
            string baseUrl = _configuration["OpenStreetMap:BaseUrl"];
            string url = $"{baseUrl}/driving/{startLng},{startLat};{endLng},{endLat}?overview=full&geometries=geojson";

            var response = await _httpClient.GetStringAsync(url);
            return response;
        }
    }
}
