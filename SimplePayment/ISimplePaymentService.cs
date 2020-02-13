using SimplePayment.Common.Models;

namespace SimplePayment
{
    public interface ISimplePaymentService
    {
        StartTransactionResponse StartTransaction(OrderDetails orderDetails, bool isTwoStep = false);
        OrderResponse HandlePaymentResponse(PaymentResponse paymentResponse);
        OrderResponse HandleIPNResponse(OrderResponse paymentResponse, IPNModel ipnResponse);
        OrderResponse FinishTwoStepTransaction(FinishRequest finishRequest, OrderResponse orderResponse);
    }
}
