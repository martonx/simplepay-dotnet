using System.Text.Json.Serialization;

namespace SimplePayment.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Currency
    {
        HUF = 1,
        EUR = 2,
        USD = 3
    }
}
