using smakchet.application.Constants.Order;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;

namespace smakchet.application.DTOs.Order
{
    public class OrderDto
    {
        [Required(ErrorMessage = OrderMessageConstant.RequiredNumber)]
        [RegularExpression(@"^\d+$", ErrorMessage = OrderMessageConstant.InvalidNumber)]
        public int Number { get; set; }

        [Required(ErrorMessage = OrderMessageConstant.RequiredType)]
        [OrderTypeValidation]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = OrderMessageConstant.RequiredStatus)]
        [OrderStatusValidation]
        public string Status { get; set; } = string.Empty;

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
