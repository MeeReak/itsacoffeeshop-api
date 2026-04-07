using System.Text.Json.Serialization;
using smakchet.application.Constants.Enum;
using smakchet.application.Validation;

namespace smakchet.application.DTOs.OrderItem
{
    public class OrderItemUpdateDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public string Number { get; set; } = string.Empty;

        [JsonConverter(typeof(StrictEnumConverter<OrderItemSizeEnum>))]
        public int SizeId { get; set; }

        public string Note { get; set; } = string.Empty;
    }
}
