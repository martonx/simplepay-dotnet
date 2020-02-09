using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePayment.Common.Models
{
    public class BaseOrderDetails
    {
        public string Merchant { get; set; }
        public string OrderRef { get; set; }
        public string Salt { get; set; }
        public string SDKVersion { get; set; }
        public Enums.Currency Currency { get; set; }
    }
}
