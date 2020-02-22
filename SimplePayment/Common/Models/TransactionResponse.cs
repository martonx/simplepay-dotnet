using System.Text.Json.Serialization;

namespace SimplePayment.Common.Models
{
    public class TransactionResponse
    {
        public string Salt { get; set; }
        public string Merchant { get; set; }
        public string OrderRef { get; set; }
        public string Currency { get; set; }
        public int TransactionId { get; set; }
        public string Timeout { get; set; }
        public decimal Total { get; set; }
        public string PaymentUrl { get; set; }
        public string[] ErrorCodes { get; set; }
    }
}
