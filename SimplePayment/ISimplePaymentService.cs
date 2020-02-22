using System.Threading.Tasks;
using SimplePayment.Common.Models;

namespace SimplePayment
{
    public interface ISimplePaymentService
    {
        Task<StartTransactionResponse> StartTransaction(OrderDetails orderDetails, bool isTwoStep = false);
        OrderResponse HandlePaymentResponse(PaymentResponse paymentResponse, string signiture);
        Task<OrderResponse> HandleIPNResponse(OrderResponse paymentResponse, IPNModel ipnResponse, string signiture);
        Task<OrderResponse> FinishTwoStepTransaction(FinishRequest finishRequest, OrderResponse orderResponse);
    }
}
