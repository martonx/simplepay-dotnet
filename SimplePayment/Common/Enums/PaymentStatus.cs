using System;
using System.Text.Json.Serialization;

namespace SimplePayment.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentStatus
    {
        Init = 1,
        Timeout = 2,
        Cancelled = 3,
        NotAuthorized = 4,
        InPayment = 5,
        InFraud = 6,
        AUTHORIZED = 7, 
        Fraud = 8,
        Reversed = 9,
        Refund = 10,
        Finished = 11
    }
}
