using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePayment.Common.Models
{
    public class OrderDetails : BaseOrderDetails
    {
        public string Merchant { get; set; }
        public string OrderRef { get; set; }
        public Enums.Currency Currency { get; set; }
        public string Salt { get; set; }
        public string Customer { get; set; }
        public string CustomerEmail { get; set; }
        public bool? TwoStep { get; set; }
        public string Language { get; set; }
        public string SDKVersion { get; set; }
        public string[] Methods { get; set; }
        public int Total { get; set; }
        public DateTime TimeOut { get; set; }
        public string Url { get; set; }
        public BillingDetails Invoice { get; set; }
        public OrderItem[] OrderItems { get; set; }
        public BillingDetails Delivery { get; set; }
    }
}
