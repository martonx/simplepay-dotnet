using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SimplePayment.Common.Models;
using SimplePayment.Helpers;

namespace SimplePayment
{
    public class SimplePaymentService : ISimplePaymentService
    {
        private readonly SimplePaymentSettings _simplePaymentSettings;
        private readonly SimplePaymentClient _simplePaymentClient;

        public SimplePaymentService(SimplePaymentSettings simplePaymentSettings)
        {
            _simplePaymentSettings = simplePaymentSettings;
            _simplePaymentClient = new SimplePaymentClient(new HttpClient(),_simplePaymentSettings);
        }

        public StartTransactionResponse StartTransaction(OrderDetails orderDetails, bool isTwoStep = false)
        {
            //await _simplePaymentClient.PostAsync<TransactionResponse, OrderDetails>(orderDetails, _simplePaymentClient);
            throw new NotImplementedException();
        }

        public OrderResponse HandlePaymentResponse(PaymentResponse paymentResponse)
        {
            throw new System.NotImplementedException();
        }

        public OrderResponse HandleIPNResponse(OrderResponse paymentResponse, IPNModel ipnResponse)
        {
            throw new System.NotImplementedException();
        }

        public OrderResponse FinishTwoStepTransaction(FinishRequest finishRequest, OrderResponse orderResponse)
        {
            throw new System.NotImplementedException();
        }

        private void ValidateOrderDetailsModel()
        {

        }
    }
}
