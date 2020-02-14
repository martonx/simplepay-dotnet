using NUnit.Framework;
using SimplePayment.Common.Models;

namespace SimplePayment.IntegrationTest
{
    public class BasicFlowTests
    {
        private readonly SimplePaymentService paymentService;

        public BasicFlowTests()
        {
            var settings = new SimplePaymentSettings { IsTestEnvironment = true, Merchant = "valami", SecretKey = "valami" };
            paymentService = new SimplePaymentService(settings);
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