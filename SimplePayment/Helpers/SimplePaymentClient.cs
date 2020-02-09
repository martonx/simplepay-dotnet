using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimplePayment.Helpers
{
    public class SimplePaymentClient
    {
        private readonly HttpClient httpClient;

        public SimplePaymentClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
    }
}
