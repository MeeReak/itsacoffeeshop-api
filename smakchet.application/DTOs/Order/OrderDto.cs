using smakchet.application.Constants.Enum;
using smakchet.application.Constants.Order;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;

namespace smakchet.application.DTOs.Order
{
    public class OrderDto
    {
        [Required(ErrorMessage = OrderMessageConstant.RequiredType)]
        [EnumValidation(typeof(OrderTypeEnum))]
        public OrderTypeEnum? Type { get; set; }


        public int CashierId { get; set; }
    }
}
