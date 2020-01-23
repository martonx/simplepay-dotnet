using System;

namespace SimplePayment.Common.Models
{
    public class IPNRequestModel : IPNModel
    {
        public DateTime ReceiveDate { get; set; }
    }
}
