using smakchet.application.Constants.Enum;
using smakchet.application.Constants.Order;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace smakchet.application.DTOs.OrderStatusLog
{
    public class OrderStatusLogDto
    {
        [Required(ErrorMessage = OrderMessageConstant.RequiredType)]
        [JsonConverter(typeof(StrictEnumConverter<OrderStatusEnum>))]
        public OrderStatusEnum? Status { get; set; }


        public int CashierId { get; set; }
    }
}
