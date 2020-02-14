using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace SimplePayment.Common.Models
{
    public class PaymentResponse
    {
        [JsonName("r")]
        public int ResponseCode { get; set; }
        [JsonName("t")]
        public string TransactionId { get; set; }
        [JsonName("e")]
        public string Event { get; set; }
        [JsonName("m")]
        public string Merchant { get; set; }
        [JsonName("o")]
        public string OrderId { get; set; }
    }
}
