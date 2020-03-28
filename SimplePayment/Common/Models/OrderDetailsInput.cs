using System;
using System.Collections.Generic;
using System.Text;
using SimplePayment.Common.Enums;

namespace SimplePayment.Common.Models
{
    public class OrderDetailsInput
    {
        public string OrderRef { get; set; }
        public Currency Currency { get; set; }
        public string CustomerEmail { get; set; }
        public Language Language { get; set; }
        public PaymentMethodTypes[] Methods { get; set; }
        public string Total { get; set; }
        public DateTimeOffset Timeout { get; set; }
        public string Url { get; set; }
        public bool? TwoStep { get; set; }
        public BillingDetails Invoice { get; set; }
        public OrderItem[] OrderItems { get; set; }
    }
}
