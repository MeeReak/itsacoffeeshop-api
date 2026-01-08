using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace smakchet.api.Controllers.V2026_01_01
{
    [ApiVersion("2026-01-01")]
    [Route("health")]
    [Produces("application/json")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public ActionResult<string> Health()
        {
            return Ok(new { message = "Service is running!!" });
        }
    }
}