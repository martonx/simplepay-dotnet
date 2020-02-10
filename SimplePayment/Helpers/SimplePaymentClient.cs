using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimplePayment.Common.Models;

namespace SimplePayment.Helpers
{
    public class SimplePaymentClient
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationHelper authenticationHelper;
        private readonly SimplePaymentSettings settings;

        public SimplePaymentClient(HttpClient httpClient, SimplePaymentSettings settings)
        {
            this.httpClient = httpClient;
            this.settings = settings;
            authenticationHelper = new AuthenticationHelper();
        }

        public async Task<T> PostAsync<T, TK>(TK model, string urlPart)
        {
            var jsonContent = JsonConvert.SerializeObject(model,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            GenerateSignatureToHeader(jsonContent);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(urlPart, stringContent);
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseString);
        }

        private void GenerateSignatureToHeader(string body)
        {
            var hash = authenticationHelper.HMACSHA384Encode(settings.SecretKey, body);
            httpClient.DefaultRequestHeaders.Add("Signature", hash);
        }

    }
}
