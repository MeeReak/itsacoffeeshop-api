using smakchet.application.DTOs.Payment;

namespace smakchet.application.Interfaces.IPayment
{
  public interface IPaymentService
  {
    Task<PaymentCheckOutDto> CheckOutAsync(int orderId, PaymentDto paymentDto, CancellationToken cancellationToken);
    Task<PaymentStatusResponseDto> CheckStatusAsync(
      int paymentId,
      CancellationToken cancellationToken);
    Task<PaymentReadDto?> GetPaymentOrderByIdAsync(int paymentId, CancellationToken cancellationToken);
    //Task<dynamic> Verification();
    //Task<dynamic> DecodeKHQR();
    //Task<dynamic> GenerateDeeplink();
  }
}
