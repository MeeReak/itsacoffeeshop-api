using smakchet.application.Constants.Enum;

namespace smakchet.application.DTOs.Payment
{
  public class PaymentStatusResponseDto
  {
    public int PaymentId { get; set; }
    public string ReferenceCode { get; set; } = default!;
    public PaymemtStatusEnum Status { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime ExpiredAt { get; set; }
    public string Message { get; set; } = default!;
  }
}
