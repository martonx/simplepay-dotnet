using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using SimplePayment.Common.Enums;
using SimplePayment.Common.Models;
using SimplePayment.Helpers;

namespace SimplePayment
{
    public class SimplePaymentService
    {
        private readonly AuthenticationHelper _authenticationHelper;
        private readonly SimplePaymentSettings _simplePaymentSettings;

        public SimplePaymentService(AuthenticationHelper authenticationHelper, SimplePaymentSettings simplePaymentSettings)
        {
            _authenticationHelper = authenticationHelper;
            _simplePaymentSettings = simplePaymentSettings;
        }

        public OrderResponse ValidatePaymentResponse(PaymentResponse paymentResponse, string signature)
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

            if (paymentResponse.Event.ToLower() != "success")
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
    }
}
