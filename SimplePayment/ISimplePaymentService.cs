using SimplePayment.Common.Models;

namespace SimplePayment
{
    public interface ISimplePaymentService
    {
        StartTransactionResponse StartTransaction(OrderDetails orderDetails);
    }
}
