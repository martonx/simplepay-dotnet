using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePayment.Common.Models
{
    public class RedirectUrls
    {
        public string Success { get; set; }
        public string Fail { get; set; }
        public string Timeout { get; set; }
        public string Cancel { get; set; }
    }
}
