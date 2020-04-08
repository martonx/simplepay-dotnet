using SimplePayment.Common.Enums;

namespace SimplePayment.Common.Models
{
    public class OrderResponse
    {
        public int TransactionId { get; set; }
        public OrderStatus Status { get; set; }
        public string Error { get; set; }
        public string OrderRef { get; set; }
    }
}
