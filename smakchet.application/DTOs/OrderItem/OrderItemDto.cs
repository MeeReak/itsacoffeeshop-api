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


        [Required(ErrorMessage = OrderMessageConstant.RequiredProductName)]
        [StringLength(50)]
        [MinLength(2)]
        public string ProductName { get; set; } = null!;


        [Required(ErrorMessage = OrderMessageConstant.RequiredPrice)]
        [RegularExpression(@"^\d{1,5}$|(?=^.{1,5}$)^\d+\.\d{0,2}$", ErrorMessage = OrderMessageConstant.InvalidPrice)]
        public decimal Price { get; set; }


        [Required(ErrorMessage = OrderMessageConstant.RequiredQuantity)]
        [RegularExpression(@"^\d+$", ErrorMessage = OrderMessageConstant.InvalidQuantity)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = OrderMessageConstant.RequiredSize)]
        [OrderItemSizeValidation]
        public OrderItemSizeEnum Size { get; set; }

        public string? Note { get; set; }
    }
}
