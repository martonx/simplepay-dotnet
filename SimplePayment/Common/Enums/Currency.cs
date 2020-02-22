using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SimplePayment.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Currency
    {
        [EnumMember(Value = "HUF")]
        HUF = 1,
        [EnumMember(Value = "EUR")]
        EUR = 2,
        [EnumMember(Value = "USD")]
        USD = 3
    }
}
