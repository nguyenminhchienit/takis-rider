using CORE.Infrastructure.Shared.Response;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CORE.Applications.Feature.Ride.Queries;
using CORE.Applications.Feature.Ride.Commnads;
using CORE.Applications.MessageQueue.Ride;
using CORE.Applications.Feature.ReviewRide.Commands;
using CORE.Applications.Feature.ReviewRide.Queries;
using StackExchange.Redis;

namespace RideServiceApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("ride-service/api/[controller]")]
    public class RideController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IConnectionMultiplexer redis;
        public RideController(IMediator _mediator, IConnectionMultiplexer _redis)
        {
            mediator = _mediator;
            redis = _redis;
        }

        [HttpGet("get-ride-by-id/{id}")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRideById(Guid id)
        => Ok(await mediator.Send(new GetRideByIdQueryRequest(id)).ConfigureAwait(false));

        [HttpGet("get-ride-by-user/{id}")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRideByUserId(string id)
        => Ok(await mediator.Send(new GetRidesByUserQueryRequest(id)).ConfigureAwait(false));

        [HttpGet("get-ride-for-passenger/{id}")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRideForPassenger(Guid id)
        => Ok(await mediator.Send(new GetRideForPassengerQueryRequest(id)).ConfigureAwait(false));


        [HttpGet("get-ride-for-driver/{id}")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRideForDriver(Guid id)
        => Ok(await mediator.Send(new GetRideForDriverQueryRequest(id)).ConfigureAwait(false));

        [HttpPost("request-ride")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RequestRide([FromBody] RequestRideCommandRequest request)
        => Ok(await mediator.Send(request).ConfigureAwait(false));


        [HttpPost("update-status-ride")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateStatusRide([FromBody] UpdateRideStatusCommandRequest request)
        => Ok(await mediator.Send(request).ConfigureAwait(false));


        [HttpPatch("accept-ride-for-driver")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AcceptRideFroDriver([FromBody] AcceptRiderForDriverCommandRequest request)
        => Ok(await mediator.Send(request).ConfigureAwait(false));


        [HttpPatch("cancle-ride")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CancleRide([FromBody] CancleRideCommandRequest request)
        => Ok(await mediator.Send(request).ConfigureAwait(false));


        [HttpPost("review-ride")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReviewRide([FromBody] ReviewRideCommandRequest request)
        => Ok(await mediator.Send(request).ConfigureAwait(false));

        [HttpGet("get-review-ride-of-driver")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetReviewRideOfDriver(string driverId)
        => Ok(await mediator.Send(new GetReviewRideOfDriverQueryRequest(driverId)).ConfigureAwait(false));


        [HttpPost("accept")]
        public async Task<IActionResult> AcceptRide(string driverId, Guid rideId)
        {
            // Kiểm tra nếu đã có người nhận thì từ chối
            var existingDriverId = await redis.GetDatabase().StringGetAsync($"Ride:{rideId}:AcceptedDriverId");
            if (!string.IsNullOrEmpty(existingDriverId.ToString()))
            {
                return Conflict("Đã có tài xế nhận chuyến này.");
            }

            // Ghi vào Redis
            await redis.GetDatabase().StringSetAsync($"Ride:{rideId}:AcceptedDriverId", driverId.ToString());

            return Ok("Bạn đã nhận chuyến thành công.");
        }



    }
}
