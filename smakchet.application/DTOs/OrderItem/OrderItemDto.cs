using smakchet.application.Constants.Order;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using smakchet.application.Constants.Enum;

namespace smakchet.application.DTOs.OrderItem
{
    public class OrderItemDto
    {
        [Required(ErrorMessage = OrderMessageConstant.RequiredProductId)]
        public int ProductId { get; set; }


        [Required(ErrorMessage = OrderMessageConstant.RequiredQuantity)]
        [RegularExpression(@"^\d+$", ErrorMessage = OrderMessageConstant.InvalidQuantity)]
        public int Quantity { get; set; }


        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = OrderMessageConstant.RequiredSize)]
        [OrderItemSizeValidation]
        public OrderItemSizeEnum Size { get; set; }


        public string? Note { get; set; }
    }
}
