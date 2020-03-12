﻿using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SimplePayment.Common.Enums;
using SimplePayment.Common.Models;
using SimplePayment.Helpers;

namespace SimplePayment
{
    public class SimplePaymentClient : ISimplePaymentClient
    {
        private readonly SimplePaymentSettings _simplePaymentSettings;
        private readonly HttpClientWrapper _httpClientWrapper;
        private readonly URLGeneratorHelper _urlGeneratorHelper;
        private readonly AuthenticationHelper _authenticationHelper;
        private readonly SimplePaymentService _simplePaymentService;

        public SimplePaymentClient(SimplePaymentSettings simplePaymentSettings)
        {
            _simplePaymentSettings = simplePaymentSettings;
            _httpClientWrapper = new HttpClientWrapper(new HttpClient(),_simplePaymentSettings);
            _urlGeneratorHelper = new URLGeneratorHelper(_simplePaymentSettings);
            _authenticationHelper = new AuthenticationHelper();
            _simplePaymentService = new SimplePaymentService(_authenticationHelper, _simplePaymentSettings);
        }

        public async Task<StartTransactionResponse> StartTransaction(OrderDetailsInput orderDetailsInput)
        {
            var orderDetails = new OrderDetails(orderDetailsInput);
            var result = new StartTransactionResponse();
            orderDetails.Merchant = _simplePaymentSettings.Merchant;
            orderDetails.Salt = _authenticationHelper.GenerateSalt();
            var isValidForm = ValidateOrderDetailsModel(orderDetails);
            if (!isValidForm)
            {
                result.Status = OrderStatus.ValidationError;
                result.Error = "Error during validation, please check if all required fields are populated correctly";
            }

            if (!string.IsNullOrEmpty(result.Error))
            {
                return result;
            }

            var url = _urlGeneratorHelper.GenerateUrl(URLType.StartTransaction);
            TransactionResponse response;
            try
            {
                response = await _httpClientWrapper.PostAsync<TransactionResponse, OrderDetails>(orderDetails, url);
            }
            catch (Exception ex)
            {
                result.Status = OrderStatus.ValidationError;
                result.Error = ex.Message;
                return result;
            }

            if (response.ErrorCodes != null )
            {
                result.Status = OrderStatus.TransactionStartFailed;
                var errorCodes = string.Join(",", response.ErrorCodes);
                result.Error = $"Transaction failed with error codes : {errorCodes}";
            }
            else
            {
                result.PaymentUrl = response.PaymentUrl;
                result.Timeout = response.Timeout;
                result.TransactionId = response.TransactionId;
                result.OrderRef = response.OrderRef;
                result.Status = OrderStatus.TransactionStartSuccess;
            }

            return result;
        }

        public OrderResponse ProcessPaymentResponse(PaymentResponse response, string signature)
        {
            return _simplePaymentService.ValidatePaymentResponse(response, signature);
        }

        public OrderResponse HandleIPNResponse(IPNRequestModel ipnResponse, string signature)
        {
            var result = new OrderResponse();
            var ipnModel = JsonSerializer.Deserialize<IPNModel>(JsonSerializer.Serialize(ipnResponse));
            var isValidSignature = _authenticationHelper.IsMessageValid(_simplePaymentSettings.SecretKey,
                JsonSerializer.Serialize(ipnModel),
                signature);

            if (!isValidSignature)
            {
                result.Error = "Signature validation failed";
                result.Status = OrderStatus.ValidationError;
                return result;
            }

            switch (ipnResponse.Status)
            {
                case PaymentStatus.Finished:
                case PaymentStatus.Authorised:
                    result.Status = OrderStatus.IPNSuccess;
                    break;
                case PaymentStatus.Cancelled:
                    result.Status = OrderStatus.IPNFailed;
                    result.Error = $"Payment was cancelled";
                    break;
                case PaymentStatus.Timeout:
                    result.Status = OrderStatus.IPNFailed;
                    result.Error = $"Payment timeout reached";
                    break;
                case PaymentStatus.Fraud:
                case PaymentStatus.InFraud:
                    result.Status = OrderStatus.IPNFailed;
                    result.Error = $"Fraud detection uncovered possible issue with card";
                    break;
                default:
                    result.Status = OrderStatus.IPNFailed;
                    result.Error = $"IPN failed with status {result.Status}";
                    break;
                
            }

            return result;
        }

        public async Task<OrderResponse> FinishTwoStepTransaction(FinishRequest finishRequest)
        {
            finishRequest.Merchant = _simplePaymentSettings.Merchant;
            finishRequest.Salt = _authenticationHelper.GenerateSalt();
            await _httpClientWrapper.PostAsync<string, FinishRequest>(finishRequest,
                _urlGeneratorHelper.GenerateUrl(URLType.TwoStepFinish));

            return new OrderResponse
            {
                OrderRef = finishRequest.OrderRef,
                Status = OrderStatus.SuccessfulTwoStepFinish
            };
        }

        private bool ValidateOrderDetailsModel(OrderDetails orderDetails)
        {
            return !string.IsNullOrEmpty(orderDetails.Salt) &&
                         !string.IsNullOrEmpty(orderDetails.Merchant) &&
                         !string.IsNullOrEmpty(orderDetails.OrderRef) &&
                         !string.IsNullOrEmpty(orderDetails.Currency) &&
                         !string.IsNullOrEmpty(orderDetails.CustomerEmail) &&
                         !string.IsNullOrEmpty(orderDetails.Language) &&
                         !string.IsNullOrEmpty(orderDetails.SDKVersion) &&
                         orderDetails.Methods.Length > 0 &&
                         !string.IsNullOrEmpty(orderDetails.Total) &&
                         !string.IsNullOrEmpty(orderDetails.Url) &&
                         (!string.IsNullOrEmpty(orderDetails.Invoice.Name) ||
                          !string.IsNullOrEmpty(orderDetails.Invoice.Company)) &&
                         !string.IsNullOrEmpty(orderDetails.Invoice.Country) &&
                         !string.IsNullOrEmpty(orderDetails.Invoice.State) &&
                         !string.IsNullOrEmpty(orderDetails.Invoice.City) &&
                         !string.IsNullOrEmpty(orderDetails.Invoice.Zip) &&
                         (!string.IsNullOrEmpty(orderDetails.Invoice.Address) ||
                         !string.IsNullOrEmpty(orderDetails.Invoice.Address2));
        }
    }
}
