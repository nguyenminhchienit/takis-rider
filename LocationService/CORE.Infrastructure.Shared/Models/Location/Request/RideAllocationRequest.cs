

namespace CORE.Infrastructure.Shared.Models.Location.Request
{
    public class RideAllocationRequest
    {
        public Guid RideId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
    }
}
