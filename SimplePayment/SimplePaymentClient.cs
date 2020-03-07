using System;
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

        public async Task<OrderResponse> HandleIPNResponse(OrderResponse paymentResponse, IPNModel ipnResponse, string signature)
        {
            var result = new OrderResponse();
            var isValidSignature = _authenticationHelper.IsMessageValid(_simplePaymentSettings.SecretKey,
                JsonSerializer.Serialize(ipnResponse),
                signature);

            if (!isValidSignature)
            {
                result.Error = "Signature validation failed";
                result.Status = OrderStatus.ValidationError;
                return result;
            }

            var ipnRequest = new IPNRequestModel
            {
                Salt = ipnResponse.Salt,
                FinishDate = ipnResponse.FinishDate,
                Method = ipnResponse.Method,
                Merchant = ipnResponse.Merchant,
                OrderRef = ipnResponse.OrderRef,
                PaymentDate = ipnResponse.PaymentDate,
                ReceiveDate = DateTime.Now,
                Status = ipnResponse.Status,
                TransactionId = ipnResponse.TransactionId
            };

            await _httpClientWrapper.PostAsync<string,  IPNRequestModel>(ipnRequest,
                    _urlGeneratorHelper.GenerateUrl(URLType.IPN));

            result.Status = OrderStatus.IPNSuccess;
            return result;
        }

        public async Task<OrderResponse> FinishTwoStepTransaction(FinishRequest finishRequest, OrderResponse orderResponse)
        {
            var result = new OrderResponse();
            if (orderResponse.Status != OrderStatus.IPNSuccess)
            {
                result.Status = OrderStatus.ValidationError;
                result.Error = "IPN wasn't successful";
                return result;
            }

            finishRequest.Merchant = _simplePaymentSettings.Merchant;
            finishRequest.Salt = _authenticationHelper.GenerateSalt();
            var response = await _httpClientWrapper.PostAsync<string, FinishRequest>(finishRequest,
                _urlGeneratorHelper.GenerateUrl(URLType.TwoStepFinish));

            return new OrderResponse
            {
                TransactionId = orderResponse.TransactionId,
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
