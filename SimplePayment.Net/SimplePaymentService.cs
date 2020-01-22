using SimplePayment.Net.Common.Models;
using SimplePayment.Net.Helpers;

namespace SimplePayment.Net
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
