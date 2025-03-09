

namespace CORE.Infrastructure.Shared.Models.Driver.Response
{
    public class DriverAllocationResponse
    {
        public Guid RideId { get; set; }
        public string DriverId { get; set; } = string.Empty;
        public bool Accepted { get; set; }
    }
}
