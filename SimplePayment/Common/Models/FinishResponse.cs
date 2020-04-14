using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePayment.Common.Models
{
    public class FinishResponse
    {
        public string Salt { get; set; }
        public string Merchant { get; set; }
        public string OrderRef { get; set; }
        public decimal ApproveTotal { get; set; }
        public string Currency { get; set; }
        public string SDKVersion { get; set; }
        public int[] ErrorCodes { get; set; }
    }
}