using smakchet.application.Constants.Order;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;
using smakchet.application.Constants.Enum;

namespace smakchet.application.DTOs.Order
{
    public class OrderUpdateDto
    {

        [Required(ErrorMessage = OrderMessageConstant.RequiredType)]
        [OrderTypeValidation]
        public OrderTypeEnum Type { get; set; }

        [Required(ErrorMessage = OrderMessageConstant.RequiredStatus)]
        [OrderStatusValidation]
        public OrderStatusEnum Status { get; set; }

        [Required(ErrorMessage = OrderMessageConstant.RequiredSubtotal)]
        [RegularExpression(@"^\d{1,5}$|(?=^.{1,5}$)^\d+\.\d{0,2}$", ErrorMessage = OrderMessageConstant.InvalidSubtotal)]
        public decimal Subtotal { get; set; }

        [Required(ErrorMessage = OrderMessageConstant.RequiredTax)]
        [RegularExpression(@"^\d{1,5}$|(?=^.{1,5}$)^\d+\.\d{0,2}$", ErrorMessage = OrderMessageConstant.InvalidTax)]
        public decimal Tax { get; set; }

        [Required(ErrorMessage = OrderMessageConstant.RequiredTotal)]
        [RegularExpression(@"^\d{1,5}$|(?=^.{1,5}$)^\d+\.\d{0,2}$", ErrorMessage = OrderMessageConstant.InvalidTotal)]
        public decimal Total { get; set; }

        public int CashierId { get; set; }
    }
}
