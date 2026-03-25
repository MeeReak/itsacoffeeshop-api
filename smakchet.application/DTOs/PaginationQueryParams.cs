using Microsoft.AspNetCore.Mvc;

namespace smakchet.application.DTOs
{
    public class PaginationQueryParams
    {
        public int Skip { get; set; } = 0;
        public int Top { get; set; } = 10;
        public string? Search { get; set; } = string.Empty;
        [FromQuery(Name = "is-feature")]
        public bool? IsFeature { get; set; }
    }
}
