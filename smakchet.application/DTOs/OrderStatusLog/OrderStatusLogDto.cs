using smakchet.application.Constants.Enum;
using smakchet.application.Constants.Order;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace smakchet.application.DTOs.OrderStatusLog
{
    public class OrderStatusLogDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = OrderMessageConstant.RequiredType)]
        [OrderTypeValidation]
        public OrderStatusEnum Status { get; set; }


        public int CashierId { get; set; }  
    }
}
