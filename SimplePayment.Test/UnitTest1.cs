using SimplePayment.Common.Models;
using NUnit.Framework;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;

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

        [Test]
        public void TestBillingDetailModel()
        {
            var testBillingDetails = @"{
                'Name' : 'Teszt Béla', 
                'Company' : 'Teszt Kft.', 
                'Country' : 'Magyarország', 
                'City' : 'Szeged', 
                'State' : 'Csongrád', 
                'Zip' : '6722', 
                'Address' : 'Teszt utca 7.', 
                'Address2' : '2 / 3', 
                'Phone':'36701234567'
            }";
            var testDeserializing = JsonConvert.DeserializeObject<BillingDetails>(testBillingDetails).ToString();
            var testSerializing = JsonConvert.SerializeObject(testDeserializing).Replace("\"", "");

            Assert.AreEqual(testDeserializing, testSerializing);
        }

        [Test]
        public void TestPaymentResponseModel()
        {
            var testPaymentResponse = @"{
                 'r':'0',
                 't':'99310118',
                 'e':'SUCCESS',
                 'm':'PUBLICTESTHUF',
                 'o':'101010515363456734591'
            }";
            var testDeserializing = JsonConvert.DeserializeObject<PaymentResponse>(testPaymentResponse).ToString();
            var testSerializing = JsonConvert.SerializeObject(testDeserializing).Replace("\"", "");

            Assert.AreEqual(testDeserializing, testSerializing);
        }
    }
}