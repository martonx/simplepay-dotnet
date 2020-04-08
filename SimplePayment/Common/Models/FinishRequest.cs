namespace SimplePayment.Common.Models
{
    public class FinishRequest 
    {
        public string Salt { get; set; }
        public string Merchant { get; set; }
        public string OrderRef { get; set; }
        public string OriginalTotal { get; set; }
        public string ApproveTotal { get; set; }
        public string Currency { get; set; }
        public string SDKVersion { get; set; }
    }
}
