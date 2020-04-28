using System;
using System.Text.Json.Serialization;

namespace SimplePayment.Common.Models
{
    public class IPNResponseModel : IPNModel
    {
        [JsonPropertyName("receiveDate")]
        public DateTime? ReceiveDate { get; set; }
    }
}
