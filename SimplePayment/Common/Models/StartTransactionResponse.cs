using System;
using System.Collections.Generic;
using System.Text;
using SimplePayment.Common.Enums;

namespace SimplePayment.Common.Models
{
    public class StartTransactionResponse : OrderResponse
    {
        public string Timeout { get; set; }
        public string PaymentUrl { get; set; }
    }
}
