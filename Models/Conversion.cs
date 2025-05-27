using System;

namespace CurrencyRateViewer.Models
{
    public class Conversion
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Result { get; set; }
        public DateTime Date { get; set; }
    }
}

