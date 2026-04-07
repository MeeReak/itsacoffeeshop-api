using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using smakchet.application.Constants.Enum;
using smakchet.application.DTOs.OrderItem;
using smakchet.application.Validation;

namespace smakchet.application.DTOs.Order
{
    public class OrderUpdateDto
    {
        [Required]
        [EnumValidation(typeof(OrderTypeEnum))]
        public OrderTypeEnum? Type { get; set; }

        [Required]
        public int CashierId { get; set; }
        
        public List<OrderItemUpdateDto> OrderItems { get; set; } = new();
    }
}
