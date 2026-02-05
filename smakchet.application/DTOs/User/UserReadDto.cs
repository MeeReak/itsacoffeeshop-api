namespace smakchet.application.DTOs.User
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Profile { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
