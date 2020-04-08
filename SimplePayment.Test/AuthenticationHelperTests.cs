using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using SimplePayment.Common.Models;
using SimplePayment.Helpers;

namespace SimplePayment.Test
{
    public class AuthenticationHelperTests
    {

        [Test]
        public async Task MatchExampleStringWithSerializedJsonTest()
        {
            var expectedMessage = File.ReadAllText("TestJsonFiles/Message.txt");
            var res = JsonConvert.DeserializeObject<OrderDetails>(File.ReadAllText("TestJsonFiles/AuthOrderDetails.json"));
            var message = JsonConvert.SerializeObject(res,
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() });
            Assert.AreEqual(expectedMessage, message);
        }

        [Test]
        public async Task HMACSHA384EncodeCorrectlyWorkingTest()
        {
            var res = JsonConvert.DeserializeObject<OrderDetails>(File.ReadAllText("TestJsonFiles/AuthOrderDetails.json"));
            var message = JsonConvert.SerializeObject(res,
                new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() });
            message = message.Replace("/", @"\/");
            var helper = new AuthenticationHelper();
            var merchantKey = "FxDa5w314kLlNseq2sKuVwaqZshZT5d6";
            var signature = helper.HMACSHA384Encode(merchantKey, message);
            var expected = "zXAB58TJdfpDaMa2rXUlefFYVlQQ91CXqot2Y6kcG79wMh55uv1hQphH9xt7qHFn";
            Assert.AreEqual(expected,signature);
        }

        [Test]
        public async Task GenerateSaltCorrectLengthTest()
        {
            var helper = new AuthenticationHelper();
            var salt = helper.GenerateSalt();
            Assert.AreEqual(32,salt.Length);
        }

        [Test]
        public async Task IpnCallbackSignatureHandledCorrectlyWorkingTest()
        {
            var helper = new AuthenticationHelper();
            var secretKey = "D5HrF1fyQ1joLmNO0yRS4m498iZyf32m";
            var signature = "6u2xMr8bDWXfXinNqP0Nu6fndcrEpSUHHM8B7wuOB8U8CehcX65DjHJgZO3XsH6e";
            var message = "{\"salt\":\"0XTzyPrD0P7leRv4TtIef4n3tWhsFDhO\",\"orderRef\":\"traveller-138\",\"method\":\"CARD\",\"merchant\":\"S002203\",\"finishDate\":\"2020-04-07T21:42:14+02:00\",\"paymentDate\":\"2020-04-07T21:42:14+02:00\",\"transactionId\":10253948,\"status\":\"AUTHORIZED\"}";
            var result = helper.IsMessageValid(secretKey, message, signature);
            Assert.IsTrue(result);
        }
    }
}
