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
        private SimplePaymentClient _paymentClient;
        private AuthenticationHelper authenticationHelper;

        [SetUp]
        public void Init()
        {
            var settings = new SimplePaymentSettings { IsTestEnvironment = true, Merchant = "PUBLICTESTHUF", SecretKey = "FxDa5w314kLlNseq2sKuVwaqZshZT5d6" };
            _paymentClient = new SimplePaymentClient(settings);
            authenticationHelper = new AuthenticationHelper();
        }

        [Test]
        public async Task StartTransactionTest()
        {
            var order = GenerateOrderDetails();
            var result = await _paymentClient.StartTransaction(order);
            Assert.AreEqual(result.Status, OrderStatus.TransactionStartSuccess);
            Assert.True(!string.IsNullOrEmpty(result.PaymentUrl));
        }

        [Test]
        public async Task StartTransactionShouldFailTest()
        {
            var order = GenerateOrderDetails();
            order.OrderRef = "1";
            var result = await _paymentClient.StartTransaction(order);
            Assert.AreEqual(result.Status, OrderStatus.TransactionStartFailed);
            Assert.True(result.Error.Contains("5013"));
        }

        [Test]
        public async Task HandlePaymentResponseTest()
        {
            var order = GenerateOrderDetails();
            var result = await _paymentClient.StartTransaction(order);
            var paymentResponse = GeneratePaymentResponse(result.OrderRef, result.TransactionId);
            var paymentString = JsonSerializer.Serialize(paymentResponse);
            var paymentResult = _paymentClient.ProcessPaymentResponse(paymentResponse,authenticationHelper.HMACSHA384Encode("FxDa5w314kLlNseq2sKuVwaqZshZT5d6",paymentString));
            Assert.AreEqual(OrderStatus.PaymentSuccess,paymentResult.Status);
        }

        [Test, Ignore("Not complete")]
        public async Task IPNResponseTest()
        {
            var order = GenerateOrderDetails();
            var result = await _paymentClient.StartTransaction(order);
            var paymentResponse = GeneratePaymentResponse(result.OrderRef, result.TransactionId);
            var model = GenerateIPNModel(result.OrderRef,result.TransactionId);
            var ipnString = JsonSerializer.Serialize(model);
            var paymentString = JsonSerializer.Serialize(paymentResponse);
            var paymentResult = _paymentClient.ProcessPaymentResponse(paymentResponse, authenticationHelper.HMACSHA384Encode("FxDa5w314kLlNseq2sKuVwaqZshZT5d6", paymentString));
            var ipnResult = await _paymentClient.HandleIPNResponse(paymentResult, model,
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

        private OrderDetailsInput GenerateOrderDetails()
        {
            return new OrderDetailsInput()
            {
                Currency = Currency.HUF,
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
                Methods = new []{PaymentMethodTypes.CARD},
                OrderRef = new Random().Next(100000,900000).ToString(),
                Total = "1",
                Timeout = DateTimeOffset.Now.AddMinutes(10),
                Url = "https://sdk.simplepay.hu/back.php",
            };
        }
    }
}
