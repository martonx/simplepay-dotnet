using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePayment.Common.Models
{
    public class SimplePaymentSettings
    {
        public string Merchant { get; set; }
        public string SecretKey { get; set; }
        public bool IsTestEnvironment { get; set; }
    }
}
