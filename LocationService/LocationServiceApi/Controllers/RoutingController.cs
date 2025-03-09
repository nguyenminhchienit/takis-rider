using CORE.Applications.Feature.Location.Queries;
using CORE.Infrastructure.Shared.Response;
using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using CORE.Applications.Feature.Routing.Queries;

namespace LocationServiceApi.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("routing/api/[controller]")]
    public class RoutingController : ControllerBase
    {

        private readonly IMediator mediator;
        public RoutingController(IMediator _mediator)
        {
            mediator = _mediator;
        }


        [HttpGet("get-routing-location/{startLat}/{startLng}/{endLat}/{endLng}")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindNearestDriverLocation(double startLat, double startLng, double endLat, double endLng)
        => Ok(await mediator.Send(new RoutingDistantsQueryRequest(startLat,startLng, endLat, endLng)).ConfigureAwait(false));
    }
}
