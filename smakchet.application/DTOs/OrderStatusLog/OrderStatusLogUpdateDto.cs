using smakchet.application.Constants.Enum;
using smakchet.application.Constants.Order;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;

namespace smakchet.application.DTOs.OrderStatusLog
{
    public class OrderStatusLogUpdateDto
    {
        [Required(ErrorMessage = OrderMessageConstant.RequiredType)]
        [EnumValidation(typeof(OrderStatusEnum))]
        public OrderStatusEnum? Status { get; set; }


        public int CashierId { get; set; }
    }
}
