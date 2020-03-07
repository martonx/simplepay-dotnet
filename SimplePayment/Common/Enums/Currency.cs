using System.Runtime.Serialization;

namespace SimplePayment.Common.Enums
{
    public class Currency
    {
        public string CurrencyText;

        public Currency(string currency)
        {
            CurrencyText = currency;
        }

        public static Currency HUF => new Currency("HUF");
        public static Currency EUR = new Currency("EUR");
        public static Currency USD = new Currency("USD");
    }
}
