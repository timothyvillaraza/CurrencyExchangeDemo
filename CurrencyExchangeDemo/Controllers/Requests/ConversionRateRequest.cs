namespace CurrencyExchangeDemo.Controllers.Requests
{
    public class ConversionRateRequest
    {
        public string SourceCurrencyName { get; set; }
        public string TargetCurrencyName { get; set; }
        public DateTime Date { get; set; }
    }
}
