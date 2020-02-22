﻿using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SimplePayment.Common.Models;
using System.Text.Json;

namespace SimplePayment.Helpers
{
    public class SimplePaymentClient
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationHelper authenticationHelper;
        private readonly SimplePaymentSettings settings;

        public SimplePaymentClient(HttpClient httpClient, SimplePaymentSettings settings)
        {
            this.httpClient = httpClient;
            this.settings = settings;
            authenticationHelper = new AuthenticationHelper();
        }

        public async Task<T> PostAsync<T, TK>(TK model, string url)
        {
            var jsonContent = JsonSerializer.Serialize(model, CustomJsonConverter.CustomJsonOptions());
            jsonContent = jsonContent.Replace("/", @"\/");
            GenerateSignatureToHeader(jsonContent);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            var responseSignature = response.Headers.GetValues("Signature").FirstOrDefault();
            var responseString = await response.Content.ReadAsStringAsync();

            if (!authenticationHelper.IsMessageValid(settings.SecretKey,responseString,responseSignature))
            {
                throw new Exception("Response signature doesn't match the generated!");
            }

            return JsonSerializer.Deserialize<T>(responseString, CustomJsonConverter.CustomJsonOptions());
        }

        private void GenerateSignatureToHeader(string body)
        {
            var hash = authenticationHelper.HMACSHA384Encode(settings.SecretKey, body);
            httpClient.DefaultRequestHeaders.Add("Signature", hash);
        }

    }
}
