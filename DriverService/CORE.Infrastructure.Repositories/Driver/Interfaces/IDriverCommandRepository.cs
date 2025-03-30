using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Driver.Commands;
using CORE.Infrastructure.Repositories.Driver.Producer;
using CORE.Infrastructure.Shared.Models.Driver.Request;
using CORE.Infrastructure.Shared.Models.Driver.Response;

namespace CORE.Infrastructure.Repositories.Driver.Interfaces
{
    public interface IDriverCommandRepository
    {
        Task<DriverModel> RegisterDriverAsync(DriverModelRequest driverDto);
        Task<bool> UpdateDriverStatusAsync(string driverId, DriverStatus status);


        Task<bool> FindAndAssignDriverAsync(RideAllocationRequest request);
        void ConfirmDriverAssignment(DriverAllocationResponse response);
        Task AssignNextDriver(Guid rideId);
        Task<bool> FindAndAssignDriverAsyncNew(Dictionary<Guid, List<DriverInfo>> rideDriverQueue);
    }
}
