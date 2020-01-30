using SimplePayment.Common.Models;
using NUnit.Framework;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace SimplePayment.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            TestBillingDetailModel();
            TestPaymentResponseModel();
        }
        public string RemoveWhiteSpace(string json)
        {
            return Regex.Replace(json, @"(""[^""\\]*(?:\\.[^""\\]*)*"")|\s+", "$1");
        }

        [Test]
        public void TestBillingDetailModel()
        {
            var billingDetailsJson = @"{
                ""Name"" : ""Teszt Béla"", 
                ""Company"" : ""Teszt Kft."", 
                ""Country"" : ""Magyarország"", 
                ""City"" : ""Szeged"", 
                ""State"" : ""Csongrád"", 
                ""Zip"" : ""6722"", 
                ""Address"" : ""Teszt utca 7."", 
                ""Address2"" : ""2 / 3"", 
                ""Phone"":""36701234567""
            }";
            var billingDetails = JsonConvert.DeserializeObject<BillingDetails>(billingDetailsJson);
            var billingDetailsSerialized = JsonConvert.SerializeObject(billingDetails);

            Assert.IsTrue(
                string.Equals(billingDetails.Name, "Teszt Béla") ||
                string.Equals(billingDetails.Company, "Teszt Kft.") ||
                string.Equals(billingDetails.Country, "Magyarország") ||
                string.Equals(billingDetails.City, "Szeged") ||
                string.Equals(billingDetails.State, "Csongrád") ||
                string.Equals(billingDetails.Zip, "6722") ||
                string.Equals(billingDetails.Address, "Teszt utca 7.") ||
                string.Equals(billingDetails.Address2, "2 / 3") ||
                string.Equals(billingDetails.Phone, "36701234567")
            );
            Assert.AreEqual(RemoveWhiteSpace(billingDetailsJson), billingDetailsSerialized);
        }

        [Test]
        public void TestPaymentResponseModel()
        {
            var paymentResponseJson = @"{
                 ""r"":""0"",
                 ""t"":""99310118"",
                 ""e"":""SUCCESS"",
                 ""m"":""PUBLICTESTHUF"",
                 ""o"":""101010515363456734591""
            }";
            var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(paymentResponseJson);
            var paymentResponseSerialized = JsonConvert.SerializeObject(paymentResponse);


            Assert.IsTrue(
                string.Equals(paymentResponse.ResponseCode, "0") ||
                string.Equals(paymentResponse.TransactionId, "99310118") ||
                string.Equals(paymentResponse.Event, "SUCCESS") ||
                string.Equals(paymentResponse.Merchant, "PUBLICTESTHUF") ||
                string.Equals(paymentResponse.OrderId, "101010515363456734591")
            );
            Assert.AreEqual(RemoveWhiteSpace(paymentResponseJson), paymentResponseSerialized);
        }
    }
}