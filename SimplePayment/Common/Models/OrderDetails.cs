using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SimplePayment.Common.Enums;

namespace SimplePayment.Common.Models
{
    public class OrderDetails
    {
        public string Salt { get; set; }
        public string Merchant { get; set; }
        public string OrderRef { get; set; }
        public string Currency { get; set; }
        public string CustomerEmail { get; set; }
        public string Language { get; set; }
        public string SDKVersion { get; set; }
        public string[] Methods { get; set; }
        public string Total { get; set; }
        public DateTime Timeout { get; set; }
        public string Url { get; set; }
        public bool? TwoStep { get; set; }
        public BillingDetails Invoice { get; set; }
        public OrderItem[] OrderItems { get; set; }
    }
}
