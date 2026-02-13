using smakchet.application.Constants.Enum;
using System.Text.Json.Serialization;

namespace smakchet.application.DTOs.OrderItem
{
    public class OrderItemUpdateDto
    {

        public int Quantity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderItemSizeEnum Size { get; set; }

        public string Note { get; set; } = string.Empty;
    }
}
