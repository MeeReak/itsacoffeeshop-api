using smakchet.application.Constants.Enum;
using smakchet.application.Constants.Order;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace smakchet.application.DTOs.Order
{
    public class OrderUpdateDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = OrderMessageConstant.RequiredType)]
        [OrderTypeValidation]
        public OrderTypeEnum Type { get; set; }
    }
}
