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
            // Public test credentials
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
        public async Task StartTransactionValidationFail()
        {
            var order = GenerateOrderDetails();
            order.CustomerEmail = "";
            var result = await _paymentClient.StartTransaction(order);
            Assert.AreEqual(result.Status, OrderStatus.ValidationError);
            Assert.True(string.IsNullOrEmpty(result.PaymentUrl));
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
            var paymentResult = _paymentClient.ProcessPaymentResponse(paymentResponse, authenticationHelper.HMACSHA384Encode("FxDa5w314kLlNseq2sKuVwaqZshZT5d6", paymentString));
            Assert.AreEqual(OrderStatus.PaymentSuccess, paymentResult.Status);
        }

        [Test]
        public async Task IPNResponseTest()
        public async Task HandlePaymentResponseShouldFailTest()
        {
            var order = GenerateOrderDetails();
            var result = await _paymentClient.StartTransaction(order);
            var paymentResponse = GeneratePaymentResponse(result.OrderRef, result.TransactionId);
            paymentResponse.Event = "FAILED";
            var paymentString = JsonSerializer.Serialize(paymentResponse);
            var paymentResult = _paymentClient.ProcessPaymentResponse(paymentResponse, authenticationHelper.HMACSHA384Encode("FxDa5w314kLlNseq2sKuVwaqZshZT5d6", paymentString));
            Assert.AreEqual(OrderStatus.PaymentFailed, paymentResult.Status);
        }

        [Test]
        public void IPNResponseTest()
        {
            var model = GenerateIPNModel();
            var ipnString = JsonSerializer.Serialize(model);
            var hash = authenticationHelper.HMACSHA384Encode("FxDa5w314kLlNseq2sKuVwaqZshZT5d6", ipnString);
            var requestModel = JsonSerializer.Deserialize<IPNRequestModel>(JsonSerializer.Serialize(model));
            requestModel.ReceiveDate = DateTime.Now;
            var ipnResult = _paymentClient.HandleIPNResponse(requestModel, hash);
            Assert.IsTrue(ipnResult.IsSuccessful);
            var resultSignatureExpected = authenticationHelper.HMACSHA384Encode("FxDa5w314kLlNseq2sKuVwaqZshZT5d6", JsonSerializer.Serialize(requestModel));
            Assert.AreEqual(resultSignatureExpected, ipnResult.Signature);
        }

        [Test]
        public void IPNResponseShouldFailTest()
        {
            var model = GenerateIPNModel();
            model.Status = PaymentStatus.Cancelled;
            var ipnString = JsonSerializer.Serialize(model);
            var hash = authenticationHelper.HMACSHA384Encode("FxDa5w314kLlNseq2sKuVwaqZshZT5d6", ipnString);
            var requestModel = JsonSerializer.Deserialize<IPNRequestModel>(JsonSerializer.Serialize(model));
            requestModel.ReceiveDate = DateTime.Now;
            var ipnResult = _paymentClient.HandleIPNResponse(requestModel, hash);
            Assert.IsFalse(ipnResult.IsSuccessful);
        }

        private IPNModel GenerateIPNModel()
        {
            return new IPNModel()
            {
                FinishDate = DateTime.Now.AddMinutes(5),
                Merchant = "PUBLICTESTHUF",
                Method = "CARD",
                PaymentDate = DateTime.Now,
                OrderRef = new Random().Next(100000, 900000).ToString(),
                Salt = authenticationHelper.GenerateSalt(),
                Status = PaymentStatus.Finished,
                TransactionId = new Random().Next(100000, 900000)
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
                Language = Language.HU,
                Methods = new[] { PaymentMethodTypes.CARD },
                OrderRef = new Random().Next(100000, 900000).ToString(),
                Total = "1",
                Timeout = DateTimeOffset.Now.AddMinutes(10),
                Url = "https://sdk.simplepay.hu/back.php",
            };
        }
    }
}