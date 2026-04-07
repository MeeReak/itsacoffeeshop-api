using smakchet.application.Constants.Enum;
using smakchet.application.Constants.Order;
using smakchet.application.DTOs.OrderItem;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace smakchet.application.DTOs.Order
{
    public class OrderBaseDto
    {
        [JsonConverter(typeof(StrictEnumConverter<OrderTypeEnum>))]
        [Required(ErrorMessage = OrderMessageConstant.RequiredType)]
        public OrderTypeEnum Type { get; set; }
        public int CashierId { get; set; }
        public List<OrderItemBaseDto> OrderItems { get; set; } = new List<OrderItemBaseDto>();
    }
}
