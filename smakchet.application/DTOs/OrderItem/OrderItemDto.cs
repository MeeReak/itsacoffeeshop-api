using smakchet.application.Constants.Order;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;
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


        [Required(ErrorMessage = OrderMessageConstant.RequiredSize)]
        [EnumValidation(typeof(OrderItemSizeEnum))]
        public OrderItemSizeEnum? Size { get; set; }


        public string? Note { get; set; }

        [Required(ErrorMessage = OrderMessageConstant.RequiredNumber)]
        public string Number { get; set; } = string.Empty;
    }
}
