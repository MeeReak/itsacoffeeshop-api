using smakchet.application.Constants.Enum;
using smakchet.application.Constants.Order;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace smakchet.application.DTOs.Order
{
    public class OrderDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = OrderMessageConstant.RequiredType)]
        [EnumValidation(typeof(OrderTypeEnum))]
        public OrderTypeEnum Type { get; set; }


        public int CashierId { get; set; }
    }
}
