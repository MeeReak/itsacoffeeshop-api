namespace smakchet.application.DTOs.Error
{
    public class ErrorDto
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<ErrorDetailDto> Details { get; set; } = new List<ErrorDetailDto>();
    }
}
