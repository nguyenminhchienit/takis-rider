using CORE.Infrastructure.Shared.Response;
using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using CORE.Applications.Feature.Driver.Command;

namespace DriverServiceApi.Controllers
{

    [ApiExplorerSettings(GroupName = "v1")]
    [Route("driver-service/api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly IMediator mediator;
        public DriverController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpPatch("update-status-driver")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(DriverUpdateStatusCommandRequest request)
        => Ok(await mediator.Send(request).ConfigureAwait(false));

    }
}
