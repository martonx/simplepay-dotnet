using SimplePayment.Common.Models;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.IO;
using System;
using SimplePayment.Common.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonOptions = SimplePayment.Helpers.CustomJsonConverter;

namespace SimplePayment.Test
{
    public class Tests
    {
        private readonly JsonSerializerOptions customJsonOptions = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Converters =
            {
                new JsonOptions.DateTimeConverter(),
                new JsonOptions.IntToStringConverter(),
                new JsonOptions.LongToStringConverter()
            }
        };

        [Test]
        public void TestBillingDetailModel()
        {
            var billingDetailsJson = ReadJson("BillingDetails");
            var billingDetails = JsonSerializer.Deserialize<BillingDetails>(billingDetailsJson, customJsonOptions);
            var billingDetailsSerialized = JsonSerializer.Serialize(billingDetails);

            Assert.AreEqual(billingDetails.Name, "Teszt Béla");
            Assert.AreEqual(billingDetails.Company, "Teszt Kft.");
            Assert.AreEqual(billingDetails.Country, "Magyarország");
            Assert.AreEqual(billingDetails.City, "Szeged");
            Assert.AreEqual(billingDetails.State, "Csongrád");
            Assert.AreEqual(billingDetails.Zip, "6722");
            Assert.AreEqual(billingDetails.Address, "Teszt utca 7.");
            Assert.AreEqual(billingDetails.Address2, "2 / 3");
            Assert.AreEqual(billingDetails.Phone, "36701234567");
        }

        [Test]
        public void TestIPNModel()
        {
            var IPNJson = ReadJson("IPN");
            var IPN = JsonSerializer.Deserialize<IPNModel>(IPNJson, customJsonOptions); 

            Assert.AreEqual(IPN.Salt, "223G0O18VAqdLhQYbJz73adT36YzLtak");
            Assert.AreEqual(IPN.OrderRef, "101010515363456734591");
            Assert.AreEqual(IPN.Method, "CARD");
            Assert.AreEqual(IPN.Merchant, "PUBLICTESTHUF");
            Assert.AreEqual(IPN.FinishDate, DateTime.Parse("2018-09-07T20:46:18+0200"));
            Assert.AreEqual(IPN.PaymentDate, DateTime.Parse("2018-09-07T20:41:13+0200"));
            Assert.AreEqual(IPN.TransactionId, 99310118);
            Assert.AreEqual(IPN.Status, PaymentStatus.Finished);

        }

        [Test]
        public void TestPaymentResponseModel()
        {
            var paymentResponseJson = ReadJson("PaymentResponse");
            var paymentResponse = JsonSerializer.Deserialize<PaymentResponse>(paymentResponseJson, customJsonOptions);
            var paymentResponseSerialized = JsonSerializer.Serialize(paymentResponse);

            Assert.AreEqual(paymentResponse.ResponseCode, 0);
            Assert.AreEqual(paymentResponse.TransactionId, "99310118");
            Assert.AreEqual(paymentResponse.Event, "SUCCESS");
            Assert.AreEqual(paymentResponse.Merchant, "PUBLICTESTHUF");
            Assert.AreEqual(paymentResponse.OrderId, "101010515363456734591");
        }

        [Test]
        public void TestOrderDetailsModel()
        {
            var orderDetailsJson = ReadJson("OrderDetails");
            var orderDetails = JsonSerializer.Deserialize<OrderDetails>(orderDetailsJson, customJsonOptions);
            var orderDetailsSerialized = JsonSerializer.Serialize(orderDetails);

            Assert.AreEqual(orderDetails.Merchant, "PUBLICTESTHUF");
            Assert.AreEqual(orderDetails.OrderRef, "101010515363456734591");
            Assert.AreEqual(orderDetails.CustomerEmail, "sdk_test@otpmobil.com");
            Assert.AreEqual(orderDetails.Language, "HU");
            Assert.AreEqual(orderDetails.Currency, Currency.HUF);
            Assert.AreEqual(orderDetails.Total, 100);
            Assert.AreEqual(orderDetails.TwoStep, true);
            Assert.AreEqual(orderDetails.Salt, "d471d2fb24c5a395563ff60f8ba769d1");
            Assert.AreEqual(orderDetails.Methods[0], "CARD");
            Assert.AreEqual(orderDetails.Invoice.ToString(), JsonSerializer.Deserialize<BillingDetails>(ReadJson("BillingDetails")).ToString());
            Assert.AreEqual(orderDetails.TimeOut, DateTime.Parse("2018-09-07T20:51:13+00:00"));
            Assert.AreEqual(orderDetails.Url, "http://simplepaytestshop.hu/back.php");
            Assert.AreEqual(orderDetails.SDKVersion, "SimplePay_PHP_SDK_2.0_180906");
            Assert.AreEqual(orderDetails.OrderItems[0].ToString(), JsonSerializer.Deserialize<OrderItem>(ReadJson("OrderItem")).ToString());
        }

        [Test]
        public void TestOrderItemModel()
        {
            var orderItemJson = ReadJson("OrderItem");
            var orderItem = JsonSerializer.Deserialize<OrderItem>(orderItemJson, customJsonOptions);
            var orderItemSerialized = JsonSerializer.Serialize(orderItem);

            Assert.AreEqual(orderItem.Ref, "Nagy Usa Körút");
            Assert.AreEqual(orderItem.Title, "szép út");
            Assert.AreEqual(orderItem.Description, "nagyon szép");
            Assert.AreEqual(orderItem.Amount, 1);
            Assert.AreEqual(orderItem.Price, 959000);
            Assert.AreEqual(orderItem.Tax, 210000);
        }

        private string RemoveWhiteSpace(string json)
        {
            return Regex.Replace(json, @"(""[^""\\]*(?:\\.[^""\\]*)*"")|\s+", "$1");
        }

        private string ReadJson(string jsonFile)
        {
            var jsonResult = File.ReadAllText($@"TestJsonFiles\{jsonFile}Json.json");

            return RemoveWhiteSpace(jsonResult);
        }
    }
}