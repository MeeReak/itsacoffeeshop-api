using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Error;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.Size;
using smakchet.application.Interfaces.ISize;

namespace smakchet.api.Controllers.V2026_01_01
{
    [ApiVersion("2026-01-01")]
    [Route("sizes")]
    [Produces("application/json")]
    [ApiController]
    public class SizeController(ISizeService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagingDto<SizeReadDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPag([FromQuery] PaginationQueryParams param)
        {
            var response = await service.GetPagedAsync(param);
            return StatusCode(StatusCodes.Status200OK, response);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SizeReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
        {

            var response = await service.GetByIdAsync(id, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}