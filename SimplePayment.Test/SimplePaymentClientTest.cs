using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NUnit.Framework;
using SimplePayment.Common.Enums;
using SimplePayment.Common.Models;
using SimplePayment.Helpers;

namespace SimplePayment.Test
{
    public class SimplePaymentClientTest
    {
        private SimplePaymentService paymentService;
        private AuthenticationHelper authenticationHelper;

        [SetUp]
        public void Init()
        {
            var settings = new SimplePaymentSettings { IsTestEnvironment = true, Merchant = "PUBLICTESTHUF", SecretKey = "FxDa5w314kLlNseq2sKuVwaqZshZT5d6" };
            paymentService = new SimplePaymentService(settings);
            authenticationHelper = new AuthenticationHelper();
        }

        [Test]
        public async Task StartTransactionTest()
        {
            var order = GenerateOrderDetails();
            var result = await paymentService.StartTransaction(order, false);
            Assert.AreEqual(result.Status, OrderStatus.TransactionStartSuccess);
            Assert.True(!string.IsNullOrEmpty(result.PaymentUrl));
        }

        [Test]
        public async Task HandlePaymentResponseTest()
        {
            var order = GenerateOrderDetails();
            var result = await paymentService.StartTransaction(order, false);
            var paymentResponse = GeneratePaymentResponse(result.OrderRef, result.TransactionId);
            var paymentString = JsonSerializer.Serialize(paymentResponse);
            var paymentResult = paymentService.HandlePaymentResponse(paymentResponse,authenticationHelper.HMACSHA384Encode("FxDa5w314kLlNseq2sKuVwaqZshZT5d6",paymentString));
            Assert.AreEqual(OrderStatus.PaymentSuccess,paymentResult.Status);
        }

        [Test, Ignore("Not complete")]
        public async Task IPNResponseTest()
        {
            var order = GenerateOrderDetails();
            var result = await paymentService.StartTransaction(order, false);
            var paymentResponse = GeneratePaymentResponse(result.OrderRef, result.TransactionId);
            var model = GenerateIPNModel(result.OrderRef,result.TransactionId);
            var ipnString = JsonSerializer.Serialize(model);
            var paymentString = JsonSerializer.Serialize(paymentResponse);
            var paymentResult = paymentService.HandlePaymentResponse(paymentResponse, authenticationHelper.HMACSHA384Encode("FxDa5w314kLlNseq2sKuVwaqZshZT5d6", paymentString));
            var ipnResult = await paymentService.HandleIPNResponse(paymentResult, model,
                authenticationHelper.HMACSHA384Encode("FxDa5w314kLlNseq2sKuVwaqZshZT5d6", ipnString));
            Assert.AreEqual(OrderStatus.IPNSuccess, ipnResult.Status);
        }

        private IPNModel GenerateIPNModel(string orderID, int transactionID)
        {
            return new IPNModel()
            {
                FinishDate = DateTime.Now.AddMinutes(5),
                Merchant = "PUBLICTESTHUF",
                Method = "CARD",
                PaymentDate = DateTime.Now,
                OrderRef = orderID,
                Salt = authenticationHelper.GenerateSalt(),
                Status = PaymentStatus.Finished,
                TransactionId = transactionID
            };
        }

        private PaymentResponse GeneratePaymentResponse(string orderID, int transactionID)
        {
            return new PaymentResponse
            {
                Event = "SUCCESS",
                Merchant = "PUBLICTESTHUF",
                OrderId = orderID,
                ResponseCode = 0,
                TransactionId = transactionID
            };
        }

        private OrderDetails GenerateOrderDetails()
        {
            return new OrderDetails
            {
                Currency = "HUF",
                CustomerEmail = "test@test.com",
                Invoice = new BillingDetails()
                {
                    Address = "Test",
                    Address2 = "Test",
                    City = "Budapest",
                    Company = "Test",
                    Country = "hu",
                    Name = "Test",
                    Phone = "06201234567",
                    State = "Budapest",
                    Zip = "1222"
                },
                Language = "HU",
                Methods = new []{"CARD"},
                OrderRef = new Random().Next(100000,900000).ToString(),
                SDKVersion = "Test",
                Total = "1",
                Timeout = DateTime.Parse("2020-09-11T19:14:08+00:00"),
                Url = "https://sdk.simplepay.hu/back.php",
                Salt = "126dac8a12693a6475c7c24143024ef8"
            };
        }
    }
}
