using System;
using System.Collections.Generic;
using System.Text;
using SimplePayment.Common.Enums;

namespace SimplePayment.Common.Models
{
    public class OrderResponse
    {
        public string TransactionId { get; set; }
        public OrderStatus Status { get; set; }
        public string Error { get; set; }
        public string OrderRef { get; set; }
    }
}
