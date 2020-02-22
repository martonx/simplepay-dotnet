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
    }
}
