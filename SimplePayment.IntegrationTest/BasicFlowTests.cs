using NUnit.Framework;
using SimplePayment.Common.Models;

namespace SimplePayment.IntegrationTest
{
    public class BasicFlowTests
    {
        private readonly SimplePaymentClient _paymentClient;

        public BasicFlowTests()
        {
            var settings = new SimplePaymentSettings { IsTestEnvironment = true, Merchant = "PUBLICTESTHUF", SecretKey = "FxDa5w314kLlNseq2sKuVwaqZshZT5d6" };
            _paymentClient = new SimplePaymentClient(settings);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InitNormalPaymentTest()
        {
            Assert.Pass();
        }

        [Test]
        public void InitDelayedPaymentWithSuccessFinalizationTest()
        {
            Assert.Pass();
        }

        [Test]
        public void InitDelayedPaymentWithCancelledFinalizationTest()
        {
            Assert.Pass();
        }

        [Test]
        public void CallbackTest()
        {
            Assert.Pass();
        }
    }
}