﻿using System;

namespace SimplePayment.Common.Models
{
    public class IPNModel
    {
        public string Salt { get; set; }
        public string OrderRef { get; set; }
        public string Method { get; set; }
        public string Merchant { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public int TransactionId { get; set; }
        public string Status { get; set; }
    }
}
