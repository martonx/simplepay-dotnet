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
            _simplePaymentClient = new SimplePaymentClient();
        }
    }
}
