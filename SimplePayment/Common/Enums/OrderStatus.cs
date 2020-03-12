namespace SimplePayment.Common.Enums
{
    public enum OrderStatus
    {
        TransactionStartSuccess = 1,
        TransactionStartFailed = 2,
        ValidationError = 3,
        PaymentSuccess = 4,
        PaymentFailed = 5,
        PaymentCancelled = 6,
        PaymentTimeout = 7,
        IPNSuccess = 8,
        IPNFailed = 9,
        PaymentPending = 10,
        SuccessfulTwoStepFinish = 11
    }
}
 