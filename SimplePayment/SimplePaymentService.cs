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
            _simplePaymentClient = new SimplePaymentClient(new HttpClient());
        }

        public StartTransactionResponse StartTransaction(OrderDetails orderDetails)
        {
            throw new System.NotImplementedException();
        }
    }
}
