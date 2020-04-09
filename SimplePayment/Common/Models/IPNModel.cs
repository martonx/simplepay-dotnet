using System;
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace SimplePayment.Common.Models
{
    public class IPNModel
    {
        [JsonName("salt")]
        public string Salt { get; set; }
        [JsonName("orderRef")]
        public string OrderRef { get; set; }
        [JsonName("method")]
        public string Method { get; set; }
        [JsonName("merchant")]
        public string Merchant { get; set; }
        [JsonName("finishDate")]
        public DateTime FinishDate { get; set; }
        [JsonName("paymentDate")]
        public DateTime PaymentDate { get; set; }
        [JsonName("transactionId")]
        public int TransactionId { get; set; }
        [JsonName("status")]
        public Enums.PaymentStatus Status { get; set; }
    }
}
