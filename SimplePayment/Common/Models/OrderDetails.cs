using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePayment.Common.Models
{
    public class OrderDetails : BaseOrderDetails
    {
        public string CustomerEmail { get; set; }
        public string Language { get; set; }
        public string[] Methods { get; set; }
        public bool? TwoStep { get; set; }
        public int Total { get; set; }
        public DateTime TimeOut { get; set; }
        public string Url { get; set; }
        public BillingDetails Invoice { get; set; }
        public OrderItem[] OrderItems { get; set; }
    }
}
