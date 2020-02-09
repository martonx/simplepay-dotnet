using System;
using System.Collections.Generic;
using System.Text;
using SimplePayment.Common.Enums;

namespace SimplePayment.Common.Models
{
    public class StartTransactionResponse
    {
        public string Timeout { get; set; }
        public string PaymentUrl { get; set; }
        public string TransactionId { get; set; }
        public OrderStatus Status { get; set; }
        public string Error { get; set; }
    }
}
