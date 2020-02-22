using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SimplePayment.Common.Enums;
using SimplePayment.Common.Models;
using SimplePayment.Helpers;

namespace SimplePayment
{
    public class SimplePaymentService : ISimplePaymentService
    {
        private readonly SimplePaymentSettings _simplePaymentSettings;
        private readonly SimplePaymentClient _simplePaymentClient;
        private readonly URLGeneratorHelper _urlGeneratorHelper;
        private readonly AuthenticationHelper _authenticationHelper;

        public SimplePaymentService(SimplePaymentSettings simplePaymentSettings)
        {
            _simplePaymentSettings = simplePaymentSettings;
            _simplePaymentClient = new SimplePaymentClient(new HttpClient(),_simplePaymentSettings);
            _urlGeneratorHelper = new URLGeneratorHelper(_simplePaymentSettings);
            _authenticationHelper = new AuthenticationHelper();
        }

        public async Task<StartTransactionResponse> StartTransaction(OrderDetails orderDetails, bool isTwoStep = false)
        {
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
            var response = await _simplePaymentClient.PostAsync<TransactionResponse, OrderDetails>(orderDetails, url);

            if (response.ErrorCodes != null )
            {
                result.Status = OrderStatus.TransactionStartFailed;
                result.Error = string.Join(",", response.ErrorCodes);
            }
            else
            {
                result.PaymentUrl = response.PaymentUrl;
                result.Timeout = response.Timeout;
                result.TransactionId = response.TransactionId;
                result.OrderRef = response.OrderRef;
            }

            return result;
        }

        public OrderResponse HandlePaymentResponse(PaymentResponse paymentResponse, string signature)
        {
            var response = new OrderResponse();
            var isValidSignature = _authenticationHelper.IsMessageValid(_simplePaymentSettings.SecretKey,
                JsonSerializer.Serialize(paymentResponse),
                signature);

            if (!isValidSignature)
            {
                response.Error = "Signature validation failed";
                response.Status = OrderStatus.ValidationError;
                return response;
            }

            if (paymentResponse.Event != "Success")
            {
                response.Error = $"Payment failed with status {paymentResponse.Event}";
                response.Status = OrderStatus.PaymentFailed;
                return response;
            }

            response.OrderRef = paymentResponse.OrderId;
            response.Status = OrderStatus.PaymentSuccess;
            response.TransactionId = paymentResponse.TransactionId;
            return response;
        }

        public async Task<OrderResponse> HandleIPNResponse(OrderResponse paymentResponse, IPNModel ipnResponse, string signature)
        {
            var result = new OrderResponse();
            var isValidSignature = _authenticationHelper.IsMessageValid(_simplePaymentSettings.SecretKey,
                JsonSerializer.Serialize(paymentResponse),
                signature);

            if (!isValidSignature)
            {
                result.Error = "Signature validation failed";
                result.Status = OrderStatus.ValidationError;
                return result;
            }

            var request = (IPNRequestModel) ipnResponse;
            await _simplePaymentClient.PostAsync<string, IPNRequestModel>(request,
                    _urlGeneratorHelper.GenerateUrl(URLType.IPN));

            result.Status = OrderStatus.PaymentSuccess;
            return result;
        }

        public OrderResponse FinishTwoStepTransaction(FinishRequest finishRequest, OrderResponse orderResponse)
        {
            throw new System.NotImplementedException();
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
                         !string.IsNullOrEmpty(orderDetails.Timeout) &&
                         !string.IsNullOrEmpty(orderDetails.Url) &&
                         (!string.IsNullOrEmpty(orderDetails.Invoice.Name) ||
                          !string.IsNullOrEmpty(orderDetails.Invoice.Company)) &&
                         !string.IsNullOrEmpty(orderDetails.Invoice.Country) &&
                         !string.IsNullOrEmpty(orderDetails.Invoice.State) &&
                         !string.IsNullOrEmpty(orderDetails.Invoice.City) &&
                         !string.IsNullOrEmpty(orderDetails.Invoice.Zip) &&
                         !string.IsNullOrEmpty(orderDetails.Invoice.Address) &&
                         !string.IsNullOrEmpty(orderDetails.Invoice.Address2);
        }
    }
}
