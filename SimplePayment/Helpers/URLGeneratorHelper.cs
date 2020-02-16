using System;
using System.Collections.Generic;
using System.Text;
using SimplePayment.Common.Enums;
using SimplePayment.Common.Models;

namespace SimplePayment.Helpers
{
    public class URLGeneratorHelper
    {
        private readonly SimplePaymentSettings settings;
        private const string TESTBASEURL = "https://sandbox.simplepay.hu/payment/v2";
        private const string LIVEBASEURL = "https://secure.simplepay.hu/payment/v2";
        private readonly Dictionary<URLType, string> urlMap = new Dictionary<URLType, string>
        {
            {URLType.StartTransaction, "start"},
            {URLType.IPN, ""},
            {URLType.TwoStepFinish, "finish"}
        };


        public URLGeneratorHelper(SimplePaymentSettings settings)
        {
            this.settings = settings;
        }

        public string GenerateUrl(URLType urlType)
        {
            var baseUrl = settings.IsTestEnvironment ? TESTBASEURL : LIVEBASEURL;
            var url = $"{baseUrl}/{urlMap[urlType]}";
            return url;
        }

    }
}
