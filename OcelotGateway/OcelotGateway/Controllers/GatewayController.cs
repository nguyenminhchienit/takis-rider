using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/gateway")]
public class GatewayController : ControllerBase
{
    [HttpGet("ride/get-ride-id/{id}")]
    public IActionResult GetRideById(Guid id)
    {
        return Ok($"This will call ride-service to get ride by id {id}");
    }

    [HttpGet("ride/get-ride-user/{id}")]
    public IActionResult GetRideByUser(Guid id)
    {
        return Ok($"This will call ride-service to get ride by user {id}");
    }

    [HttpGet("ride/get-ride-for-passenger/{id}")]
    public IActionResult GetRideForPassenger(Guid id)
    {
        return Ok($"This will call ride-service to get ride for passenger {id}");
    }
}
