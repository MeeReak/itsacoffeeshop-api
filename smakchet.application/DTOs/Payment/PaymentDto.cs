using Newtonsoft.Json;
using smakchet.application.Constants.Enum;
using smakchet.application.Constants.Order;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;

namespace smakchet.application.DTOs.Payment
{
    public class PaymentDto
    {
        [Required(ErrorMessage = OrderMessageConstant.RequiredType)]
        [EnumValidation(typeof(PaymentMethodEnum))]
        public PaymentMethodEnum? Method { get; set; }


        [Required(ErrorMessage = OrderMessageConstant.RequiredType)]
        [EnumValidation(typeof(KHQRCurrencyEnum))]
        public KHQRCurrencyEnum? Currency { get; set; }
    }


    public class CheckTransactionRequest
    {
        //[JsonProperty("md5")]
        public string md5 { get; set; } = string.Empty;
    }


    public class BakongKHQRData
    {
        [JsonProperty("MD5")]
        public string Md5 { get; set; } = string.Empty;


        [JsonProperty("QR")]
        public string Qr { get; set; } = string.Empty;

    }


    public class BakongKHQRResponse
    {
        [JsonProperty("responseCode")]
        public int ResponseCode { get; set; }


        [JsonProperty("errorCode")] 
        public string? ErrorCode { get; set; } = string.Empty;


        [JsonProperty("responseMessage")]
        public string ResponseMessage { get; set; } = string.Empty;


        [JsonProperty("data")]
        public BakongData? Data { get; set; }
    }


    public class BakongData
    {
        [JsonProperty("hash")]
        public string Hash { get; set; } = string.Empty;


        [JsonProperty("fromAccountId")]
        public string FromAccountId { get; set; } = string.Empty;


        [JsonProperty("toAccountId")]
        public string ToAccountId { get; set; } = string.Empty;


        [JsonProperty("currency")]
        public string Currency { get; set; } = string.Empty;


        [JsonProperty("amount")]
        public decimal Amount { get; set; }


        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;
    }
}
