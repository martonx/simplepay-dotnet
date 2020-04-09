using System;
using System.Net.Http;
using System.Reflection;
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
                return result;
            }

            TransactionResponse response;
            try
            {
                var url = _urlGeneratorHelper.GenerateUrl(URLType.StartTransaction);
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

        public IPNProcessResult HandleIPNResponse(IPNRequestModel model, string signature)
        {
            var result = new IPNProcessResult { IPNRequestModel = model };
            var ipnResponseString = JsonSerializer.Serialize(model, new JsonSerializerOptions { IgnoreNullValues = true });
            var isValidSignature = _authenticationHelper.IsMessageValid(_simplePaymentSettings.SecretKey, ipnResponseString, signature);

            if (!isValidSignature)
            {
                result.ErrorMessage = "Signature validation failed";
                result.IsSuccessful = false;
                return result;
            }

            model.ReceiveDate = DateTime.Now;
            result.IsSuccessful = model.Status == PaymentStatus.Finished || model.Status == PaymentStatus.AUTHORIZED;
            switch (model.Status)
            {
                case PaymentStatus.Finished:
                case PaymentStatus.AUTHORIZED:
                    break;
                case PaymentStatus.Cancelled:
                    result.ErrorMessage = $"Payment was cancelled";
                    break;
                case PaymentStatus.Timeout:
                    result.ErrorMessage = $"Payment timeout reached";
                    break;
                case PaymentStatus.Fraud:
                case PaymentStatus.InFraud:
                    result.ErrorMessage = $"Fraud detection uncovered possible issue with card";
                    break;
                default:
                    result.ErrorMessage = $"IPN failed with status {model.Status}";
                    break;
                
            }

            if (result.IsSuccessful)
            {
                result.Signature =
                    _authenticationHelper.HMACSHA384Encode(_simplePaymentSettings.SecretKey, JsonSerializer.Serialize(model));
            }

            return result;
        }

        public async Task<OrderResponse> FinishTwoStepTransaction(FinishRequestInput finishRequestInput)
        {
            var finishRequest = new FinishRequest()
            {
                ApproveTotal = finishRequestInput.ApproveTotal,
                Currency = finishRequestInput.Currency.CurrencyText,
                Merchant = _simplePaymentSettings.Merchant,
                OrderRef = finishRequestInput.OrderRef,
                OriginalTotal = finishRequestInput.OriginalTotal,
                Salt = _authenticationHelper.GenerateSalt(),
                SDKVersion = typeof(OrderDetails).Assembly
                    .GetCustomAttribute<AssemblyFileVersionAttribute>().Version
            };

            try
            {
                var response = await _httpClientWrapper.PostAsync<FinishResponse, FinishRequest>(finishRequest,
                    _urlGeneratorHelper.GenerateUrl(URLType.TwoStepFinish));
                if (response.ErrorCodes != null)
                {
                    var errorCodes = string.Join(",", response.ErrorCodes);
                    return new OrderResponse
                    {
                        Error = $"Transaction failed with error codes : {errorCodes}",
                        OrderRef = response.OrderRef,
                        Status = OrderStatus.TransactionStartFailed
                    };
                }

                return new OrderResponse
                {
                    OrderRef = response.OrderRef,
                    Status = OrderStatus.SuccessfulTwoStepFinish
                };
            }
            catch (Exception ex)
            {
                return new OrderResponse
                {
                    Error = ex.Message,
                    OrderRef = finishRequest.OrderRef,
                    Status = OrderStatus.FailedTwoStepFinish
                };
            }

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
