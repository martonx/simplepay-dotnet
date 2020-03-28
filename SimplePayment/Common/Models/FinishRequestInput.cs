using System;
using System.Collections.Generic;
using System.Text;
using SimplePayment.Common.Enums;
using SimplePayment.Common.Enums;

namespace SimplePayment.Common.Models
{
    public class FinishRequestInput
    {
        public string OriginalTotal { get; set; }
        public string ApproveTotal { get; set; }
        public string OrderRef { get; set; }
        public int TransactionId { get; set; }
        public Currency Currency { get; set; }
    }
}