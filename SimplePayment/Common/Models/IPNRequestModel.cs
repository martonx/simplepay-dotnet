using System;
using System.Text.Json.Serialization;

namespace SimplePayment.Common.Models
{
    public class IPNRequestModel : IPNModel
    {
        [JsonPropertyName("receiveDate")]
        public DateTime? ReceiveDate { get; set; }
    }
}
