using CORE.Infrastructure.Shared.Response;
using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using CORE.Applications.Feature.Location.Commands;
using CORE.Applications.Feature.Location.Queries;
using CORE.Infrastructure.Shared.Models.Location.Request;
using CORE.Infrastructure.Repositories.Location.Interfaces;

namespace LocationServiceApi.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("location-driver-service/api/[controller]")]
    public class LocationDriverController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILocationCommandRepository locationCommandRepository;
        public LocationDriverController(IMediator _mediator, ILocationCommandRepository _locationCommandRepository)
        {
            mediator = _mediator;
            locationCommandRepository = _locationCommandRepository;
        }

        [HttpPost("update-driver-location")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateDriverLocation(UpdateDriverLocationCommandRequest request)
        => Ok(await mediator.Send(request).ConfigureAwait(false));

        [HttpGet("get-driver-location/{driverId}")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDriverLocation(string driverId)
        => Ok(await mediator.Send(new GetDriverLocationQueryRequest(driverId)).ConfigureAwait(false));


        [HttpGet("find-nearest-driver-location/{latitude}/{longitude}/{radiusKm}")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindNearestDriverLocation(double latitude, double longitude, double radiusKm)
        => Ok(await mediator.Send(new FindNearestDriverQueryRequest(latitude,longitude, radiusKm)).ConfigureAwait(false));


        [HttpPost("update")]
        public async Task<IActionResult> UpdateLocation([FromBody] DriverLocationUpdateRequest request)
        {
            await locationCommandRepository.UpdateDriverLocationAsync(request.DriverId, request.Latitude, request.Longitude);
            return Ok(new { message = "Cập nhật vị trí thành công!" });
        }

        [HttpPost("online")]
        public async Task<IActionResult> SetOnline([FromBody] DriverOnlineRequest request)
        {
            await locationCommandRepository.SetDriverOnlineAsync(request.DriverId);
            await locationCommandRepository.UpdateDriverLocationAsync(request.DriverId, request.Latitude, request.Longitude);
            return Ok(new { message = "Tài xế đã Online & cập nhật vị trí!" });
        }

        [HttpPost("offline")]
        public async Task<IActionResult> SetOffline([FromBody] DriverOnlineRequest request)
        {
            await locationCommandRepository.SetDriverOfflineAsync(request.DriverId);
            return Ok(new { message = "Tài xế đã Offline!" });
        }

    }
}
