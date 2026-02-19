using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs.Error;
using smakchet.application.DTOs.Payment;
using smakchet.application.Interfaces.IPayment;

namespace smakchet.api.Controllers.V2026_01_01
{
    [ApiVersion("2026-01-01")]
    [Route("payments")]
    [Produces("application/json")]
    [ApiController]
    public class PaymentController(IPaymentService service) : ControllerBase
    {
        [HttpGet("{paymentId:int}")]
        [ProducesResponseType(typeof(PaymentReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentReadDto>> GetPaymentAsync(int paymentId, CancellationToken cancellationToken)
        {
            var payment = await service.GetPaymentOrderByIdAsync(paymentId, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, payment);
        }


        [HttpGet("{orderId:int}/checkout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CheckOutAsync(int orderId, PaymentDto paymentDto, CancellationToken cancellationToken)
        {
            var response = await service.CheckOutAsync(orderId, paymentDto, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, response);
        }


        [HttpGet("{orderId:int}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CheckStatusAsync(int orderId, CancellationToken cancellationToken)
        {
            await service.CheckStatusAsync(orderId, cancellationToken);
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}