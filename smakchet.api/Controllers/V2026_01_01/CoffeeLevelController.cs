using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Error;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.CoffeeLevel;
using smakchet.application.Interfaces.ICoffeeLevel;

namespace smakchet.api.Controllers.V2026_01_01
{
    [ApiVersion("2026-01-01")]
    [Route("coffee-levels")]
    [Produces("application/json")]
    [ApiController]
    public class CoffeeLevelController(ICoffeeLevelService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagingDto<CoffeeLevelReadDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPag([FromQuery] PaginationQueryParams param)
        {
            var response = await service.GetPagedAsync(param);
            return StatusCode(StatusCodes.Status200OK, response);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CoffeeLevelReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
        {

            var response = await service.GetByIdAsync(id, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}