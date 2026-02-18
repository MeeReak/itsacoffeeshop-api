using Newtonsoft.Json;

namespace smakchet.application.DTOs.Payment
{
    public class PaymentDto
    {
        public int Method { get; set; }
    }


    public class CheckTransactionRequest
    {
        [JsonProperty("md5")]
        public string Md5 { get; set; } = string.Empty;
    }


    public class BakongKHQRResponse
    {
        [JsonProperty("responseCode")]
        public int ResponseCode { get; set; }


        [JsonProperty("responseMessage")]
        public string ResponseMessage { get; set; } = string.Empty;


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
