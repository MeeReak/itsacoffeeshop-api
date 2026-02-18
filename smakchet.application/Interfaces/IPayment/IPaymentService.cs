using smakchet.application.DTOs.Payment;

namespace smakchet.application.Interfaces.IPayment
{
    public interface IPaymentService
    {
        Task<PaymentReadDto> GenerateKHQR(int orderId, CancellationToken cancellationToken);
        Task CheckStatus(int paymentId, CancellationToken cancellationToken);
        Task<dynamic> Verification();
        Task<dynamic> DecodeKHQR();
        Task<dynamic> GenerateDeeplink();
    }
}
