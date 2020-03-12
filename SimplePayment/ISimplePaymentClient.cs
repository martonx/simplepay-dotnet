using System.Threading.Tasks;
using SimplePayment.Common.Models;

namespace SimplePayment
{
    public interface ISimplePaymentClient
    {
        Task<StartTransactionResponse> StartTransaction(OrderDetailsInput orderDetailsInput);
        OrderResponse ProcessPaymentResponse(PaymentResponse response, string signature);
        OrderResponse HandleIPNResponse(IPNRequestModel ipnResponse, string signature);
        Task<OrderResponse> FinishTwoStepTransaction(FinishRequest finishRequest);
    }
}
