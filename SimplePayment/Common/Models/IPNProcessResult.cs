using System;
using System.Collections.Generic;
using System.Text;
using SimplePayment.Common.Enums;

namespace SimplePayment.Common.Models
{
    public class IPNProcessResult
    {
        public bool IsSuccessful { get; set; }
        public string Signature { get; set; }
        public string ErrorMessage { get; set; }
    }
}
