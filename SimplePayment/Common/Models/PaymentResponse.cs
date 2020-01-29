using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePayment.Common.Models
{
    public class PaymentResponse
    {
        [JsonProperty("r")]
        public string ResponseCode { get; set; }
        [JsonProperty("t")]
        public string TransactionId { get; set; }
        [JsonProperty("e")]
        public string Event { get; set; }
        [JsonProperty("m")]
        public string Merchant { get; set; }
        [JsonProperty("o")]
        public string OrderId { get; set; }
    }
}
