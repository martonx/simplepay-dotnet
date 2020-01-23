using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePayment.Common.Enums
{
    public enum PaymentStatus
    {
        Init = 1,
        Timeout = 2,
        Cancelled = 3,
        NotAuthorised = 4,
        InPayment = 5,
        InFraud = 6,
        Authorised = 7, 
        Fraud = 8,
        Reversed = 9,
        Refund = 10,
        Finished = 11
    }
}
