using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePayment.Common.Models
{
    public class FinishRequest : BaseOrderDetails
    {
        public string OriginalTotal { get; set; }
        public string ApproveTotal { get; set; }
    }
}
