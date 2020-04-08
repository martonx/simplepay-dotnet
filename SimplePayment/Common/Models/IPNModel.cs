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
        [JsonName("Method")]
        public string Method { get; set; }
        [JsonName("Merchant")]
        public string Merchant { get; set; }
        [JsonName("FinishDate")]
        public DateTime FinishDate { get; set; }
        [JsonName("PaymentDate")]
        public DateTime PaymentDate { get; set; }
        [JsonName("TransactionId")]
        public int TransactionId { get; set; }
        [JsonName("Status")]
        public Enums.PaymentStatus Status { get; set; }
    }
}
