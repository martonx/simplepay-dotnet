﻿using System.Threading.Tasks;
using SimplePayment.Common.Models;

namespace SimplePayment
{
    public interface ISimplePaymentClient
    {
        Task<StartTransactionResponse> StartTransaction(OrderDetailsInput orderDetailsInput);
        OrderResponse ProcessPaymentResponse(PaymentResponse response, string signature);
        IPNProcessResult HandleIPNResponse(IPNModel ipnResponse, string signature);
        Task<OrderResponse> FinishTwoStepTransaction(FinishRequestInput finishRequestInput);
    }
}
