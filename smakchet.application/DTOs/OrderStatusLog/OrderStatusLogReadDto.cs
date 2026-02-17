namespace smakchet.application.DTOs.OrderStatusLog
{
    public class OrderStatusLogReadDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int OldStatus { get; set; }
        public int NewStatus { get; set; }
        public int? ChangeBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
