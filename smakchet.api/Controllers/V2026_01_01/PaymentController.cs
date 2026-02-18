using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs.Error;
using smakchet.application.Interfaces.IPayment;

namespace smakchet.api.Controllers.V2026_01_01
{
    [ApiVersion("2026-01-01")]
    [Route("payments")]
    [Produces("application/json")]
    [ApiController]
    public class PaymentController(IPaymentService service) : ControllerBase
    {
        [HttpGet("{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GenerateKHQR(int orderId, CancellationToken cancellationToken)
        {
            var response = await service.GenerateKHQR(orderId, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("{orderId:int}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CheckStatus(int orderId, CancellationToken cancellationToken)
        {
            await service.CheckStatus(orderId, cancellationToken);
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}