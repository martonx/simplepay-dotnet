using SimplePayment.Common.Models;
using NUnit.Framework;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using System;
using SimplePayment.Common.Enums;

namespace SimplePayment.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestBillingDetailModel()
        {
            var billingDetailsJson = ReadJson("BillingDetails");
            var billingDetails = JsonConvert.DeserializeObject<BillingDetails>(billingDetailsJson);
            var billingDetailsSerialized = JsonConvert.SerializeObject(billingDetails);

            Assert.AreEqual(billingDetails.Name, "Teszt Béla");
            Assert.AreEqual(billingDetails.Company, "Teszt Kft.");
            Assert.AreEqual(billingDetails.Country, "Magyarország");
            Assert.AreEqual(billingDetails.City, "Szeged");
            Assert.AreEqual(billingDetails.State, "Csongrád");
            Assert.AreEqual(billingDetails.Zip, "6722");
            Assert.AreEqual(billingDetails.Address, "Teszt utca 7.");
            Assert.AreEqual(billingDetails.Address2, "2 / 3");
            Assert.AreEqual(billingDetails.Phone, "36701234567");

            Assert.AreEqual(RemoveWhiteSpace(billingDetailsJson), billingDetailsSerialized);
        }

        [Test]
        public void TestIPNModel()
        {
            var IPNJson = ReadJson("IPN");
            var IPN = JsonConvert.DeserializeObject<IPNModel>(IPNJson);
            var IPNSerialized = JsonConvert.SerializeObject(IPN);

            Assert.AreEqual(IPN.Salt, "223G0O18VAqdLhQYbJz73adT36YzLtak");
            Assert.AreEqual(IPN.OrderRef, "101010515363456734591");
            Assert.AreEqual(IPN.Method, "CARD");
            Assert.AreEqual(IPN.Merchant, "PUBLICTESTHUF");
            Assert.AreEqual(IPN.FinishDate, DateTime.Parse("2018-09-07T20:46:18+0200"));
            Assert.AreEqual(IPN.PaymentDate, DateTime.Parse("2018-09-07T20:41:13+0200"));
            Assert.AreEqual(IPN.TransactionId, "99310118");
            Assert.AreEqual(IPN.Status, PaymentStatus.Finished);
        }

        [Test]
        public void TestPaymentResponseModel()
        {
            var paymentResponseJson = ReadJson("PaymentResponse");
            var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(paymentResponseJson);
            var paymentResponseSerialized = JsonConvert.SerializeObject(paymentResponse);

            Assert.AreEqual(paymentResponse.ResponseCode, "0");
            Assert.AreEqual(paymentResponse.TransactionId, "99310118");
            Assert.AreEqual(paymentResponse.Event, "SUCCESS");
            Assert.AreEqual(paymentResponse.Merchant, "PUBLICTESTHUF");
            Assert.AreEqual(paymentResponse.OrderId, "101010515363456734591");

            Assert.AreEqual(RemoveWhiteSpace(paymentResponseJson), paymentResponseSerialized);
        }

        [Test]
        public void TestOrderDetailsModel()
        {
            var orderDetailsJson = ReadJson("OrderDetails");
            var orderDetails = JsonConvert.DeserializeObject<OrderDetails>(orderDetailsJson);
            var orderDetailsSerialized = JsonConvert.SerializeObject(orderDetails);

            Assert.AreEqual(orderDetails.Merchant, "PUBLICTESTHUF");
            Assert.AreEqual(orderDetails.OrderRef, "101010515363456734591");
            Assert.AreEqual(orderDetails.Customer, "v2 START Tester");
            Assert.AreEqual(orderDetails.CustomerEmail, "sdk_test@otpmobil.com");
            Assert.AreEqual(orderDetails.Language, "HU");
            Assert.AreEqual(orderDetails.Currency, Currency.HUF);
            Assert.AreEqual(orderDetails.Total, 100);
            Assert.AreEqual(orderDetails.TwoStep, true);
            Assert.AreEqual(orderDetails.Salt, "d471d2fb24c5a395563ff60f8ba769d1");
            Assert.AreEqual(orderDetails.Methods[0], "CARD");
            Assert.AreEqual(orderDetails.Invoice.ToString(), JsonConvert.DeserializeObject<BillingDetails>(ReadJson("BillingDetails")).ToString());
            Assert.AreEqual(orderDetails.Delivery.ToString(), JsonConvert.DeserializeObject<BillingDetails>(ReadJson("BillingDetails")).ToString()); // It has equal properties as BillingDetails
            Assert.AreEqual(orderDetails.TimeOut, DateTime.Parse("2018-09-07T20:51:13+00:00"));
            Assert.AreEqual(orderDetails.Url, "http://simplepaytestshop.hu/back.php");
            Assert.AreEqual(orderDetails.SDKVersion, "SimplePay_PHP_SDK_2.0_180906");
            Assert.AreEqual(orderDetails.OrderItems[0].ToString(), JsonConvert.DeserializeObject<OrderItem>(ReadJson("OrderItem")).ToString());
        }

        [Test]
        public void TestOrderItemModel()
        {
            var orderItemJson = ReadJson("OrderItem");
            var orderItem = JsonConvert.DeserializeObject<OrderItem>(orderItemJson);
            var orderItemSerialized = JsonConvert.SerializeObject(orderItem);

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
            string folderPath = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\netcoreapp3.1", "");
            var jsonResult = File.ReadAllText($@"{folderPath}\TestJsonFiles\{jsonFile}Json.txt");

            return RemoveWhiteSpace(jsonResult);
        }
    }
}