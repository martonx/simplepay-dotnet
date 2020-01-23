using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePayment.Common.Models
{
    public class PaymentResponse
    {
        public string ResponseCode { get; set; }
        public string TransactionId { get; set; }
        public string Event { get; set; }
        public string Merchant { get; set; }
        public string OrderId { get; set; }
    }
}
