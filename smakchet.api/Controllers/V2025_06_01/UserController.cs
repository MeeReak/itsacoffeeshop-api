using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs.User;
using smakchet.api.Filter;


namespace smakchet.api.Controllers.V2025_06_01;

[ServiceFilter(typeof(ApiDeprecateActionFilter))]
[ApiVersion("2025-06-01")]
[Route("users")]
[Produces("application/json")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok("Hello version 2025-06-01");
    }

}