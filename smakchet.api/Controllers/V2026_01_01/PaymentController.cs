using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs.Error;
using smakchet.application.DTOs.Payment;
using smakchet.application.DTOs.Success;
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
    [ProducesResponseType(typeof(ResponseDto<PaymentReadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentAsync(int paymentId, CancellationToken cancellationToken)
    {
      var payment = await service.GetPaymentOrderByIdAsync(paymentId, cancellationToken);
      return Ok(ResponseDto<PaymentReadDto>.Ok(payment));
    }


    [HttpPost("{orderId:int}/checkout")]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckOutAsync(
        int orderId,
        [FromBody] PaymentDto paymentDto,
        CancellationToken cancellationToken)
    {
      if (paymentDto == null)
      {
          var error = new ErrorDto { Code = "BadRequest", Message = "Payment data is required." };
          return BadRequest(ResponseDto<object>.Fail("Payment data is required.", error));
      }

      var response = await service.CheckOutAsync(orderId, paymentDto, cancellationToken);
      return Ok(ResponseDto<object>.Ok(response, "Checkout successful"));
    }


    [HttpGet("{paymentId:int}/status")]
    [ProducesResponseType(typeof(ResponseDto<PaymentStatusResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckStatusAsync(
        int paymentId,
        CancellationToken cancellationToken)
    {
      var result = await service.CheckStatusAsync(paymentId, cancellationToken);

      return Ok(ResponseDto<PaymentStatusResponseDto>.Ok(result));
    }

    [HttpPut("{paymentId:int}/status")]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(
        int paymentId,
        [FromQuery] smakchet.application.Constants.Enum.PaymemtStatusEnum status,
        CancellationToken cancellationToken)
    {
      await service.UpdatePaymentStatusAsync(paymentId, status, cancellationToken);
      return Ok(ResponseDto<object>.Ok(null, "Payment status updated successfully"));
    }
  }
}
