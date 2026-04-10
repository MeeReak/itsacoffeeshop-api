using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs.Success;

namespace smakchet.api.Controllers.V2026_01_01
{
    [ApiVersion("2026-01-01")]
    [Route("health")]
    [Produces("application/json")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
        public IActionResult Health()
        {
            return Ok(ResponseDto<object>.Ok(new { status = "Healthy" }, "Service is running!!"));
        }
    }
}