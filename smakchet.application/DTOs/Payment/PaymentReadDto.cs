namespace smakchet.application.DTOs.Payment
{
    public class PaymentReadDto
    {
        public int Id { get; set; }
        public string Qr { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
