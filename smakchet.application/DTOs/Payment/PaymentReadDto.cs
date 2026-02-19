using smakchet.application.DTOs.Order;

namespace smakchet.application.DTOs.Payment
{
    public class PaymentReadDto
    {
        public int Id { get; set; }
        public int Method { get; set; } 
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderReadDto Orders { get; set; } = new ();
    }

    public class PaymentCheckOutDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int Method { get; set; }
        public int Currency { get; set; }
        public string Qr { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; }
    }
}
