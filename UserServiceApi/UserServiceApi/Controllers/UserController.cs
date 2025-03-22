using System.Net;
using CORE.Applications.Feature.UserMain.Commands;
using CORE.Applications.Feature.UserMain.Queries;
using CORE.Infrastructure.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserServiceApi.Controllers
{
    
    [ApiVersion("1.0")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("user-service/identity/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        public UserController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] MainUserCreateCommandRequest request)
        => Ok(await mediator.Send(request).ConfigureAwait(false));


        [HttpPost("register-make-driver")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterMakeDriver([FromBody] UserRegisterMakeDriverCommandRequest request)
        => Ok(await mediator.Send(request).ConfigureAwait(false));


        [HttpPost("auth/login")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] MainUserAuthCommandRequest request)
        => Ok(await mediator.Send(request).ConfigureAwait(false));

        [HttpPost("auth/refreshtoken")]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] MainCreateRefreshToken request)
        => Ok(await mediator.Send(request).ConfigureAwait(false));


        [HttpGet("get-user-by-id/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseCus<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserById(string id)
        => Ok(await mediator.Send(new MainGetUserByIdQueryRequest(id)).ConfigureAwait(false));
    }
}
