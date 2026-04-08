using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.Ice;
using smakchet.application.Interfaces.ILookupService;

namespace smakchet.api.Controllers.V2026_01_01
{
    [ApiVersion("2026-01-01")]
    [Route("lookups")]
    [Produces("application/json")]
    [ApiController]
    public class LookUpController(ILookupService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagingDto<IceReadDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPag()
        {
            var response = await service.GetLookupAsync();
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
