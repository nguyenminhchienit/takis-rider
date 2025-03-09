using CORE.Infrastructure.Shared.Response;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CORE.Applications.Feature.Discovery.Queries;

namespace RideServiceApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("discovery-service/api/[controller]")]
    public class DiscoveryController : ControllerBase
    {
        private readonly IMediator mediator;
        public DiscoveryController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpGet("get-service/{serviceName}")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRideForPassenger(string serviceName)
        => Ok(await mediator.Send(new DiscoveryServiceQueryRequest(serviceName)).ConfigureAwait(false));


        [HttpGet("get-all-service")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRideForDriver()
        => Ok(await mediator.Send(new DiscoveryServiceGetAllQueryRequest()).ConfigureAwait(false));
    }
}
