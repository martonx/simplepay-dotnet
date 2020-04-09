namespace SimplePayment.Common.Models
{
    public class IPNProcessResult
    {
        public bool IsSuccessful { get; set; }
        public string Signature { get; set; }
        public string ErrorMessage { get; set; }
        public IPNRequestModel IPNRequestModel { get; set; }
    }
}
