using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SimplePayment.Common.Enums;

namespace SimplePayment.Common.Models
{
    public class OrderDetails
    {
        public string Salt { get; set; }
        public string Merchant { get; set; }
        public string OrderRef { get; set; }
        public string Currency { get; set; }
        public string CustomerEmail { get; set; }
        public string Language { get; set; }
        public string SDKVersion { get; set; }
        public string[] Methods { get; set; }
        public string Total { get; set; }
        public string Timeout { get; set; }
        public string Url { get; set; }
        public RedirectUrls? Urls { get; set; }
        public bool? TwoStep { get; set; }
        public BillingDetails Invoice { get; set; }
        public OrderItem[] OrderItems { get; set; }

        public OrderDetails()
        {
            
        }

        public OrderDetails(OrderDetailsInput orderDetailsInput)
        {
            SDKVersion = typeof(OrderDetails).Assembly
                .GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            OrderRef = orderDetailsInput.OrderRef;
            Currency = orderDetailsInput.Currency.CurrencyText;
            CustomerEmail = orderDetailsInput.CustomerEmail;
            Language = orderDetailsInput.Language.LanguageText;
            Methods = new string[orderDetailsInput.Methods.Length];
            for (int i = 0; i < orderDetailsInput.Methods.Length; i++)
            {
                switch (orderDetailsInput.Methods[i])
                {
                    case PaymentMethodTypes.CARD:
                        Methods[i] = "CARD";
                        break;
                }
            }

            Total = orderDetailsInput.Total;
            Timeout = orderDetailsInput.Timeout.ToString("yyyy-MM-ddTHH:mm:ssK");
            Url = orderDetailsInput.Url;
            Urls = new RedirectUrls()
            { 
                Success = orderDetailsInput.Urls.Success,
                Cancel = orderDetailsInput.Urls.Cancel,
                Fail = orderDetailsInput.Urls.Fail,
                Timeout = orderDetailsInput.Urls.Timeout
            };
            TwoStep = orderDetailsInput.TwoStep;
            Invoice = orderDetailsInput.Invoice;
            OrderItems = orderDetailsInput.OrderItems;
        }
    }
}
