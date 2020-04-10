using System;
using System.Text.Json.Serialization;

namespace SimplePayment.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentStatus
    {
        INIT = 1,
        TIMEOUT = 2,
        CANCELLED = 3,
        NOTAUTHORIZED = 4,
        INPAYMENT = 5,
        INFRAUD = 6,
        AUTHORIZED = 7, 
        FRAUD = 8,
        REVERSED = 9,
        REFUND = 10,
        FINISHED = 11
    }
}
