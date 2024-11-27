namespace CurrencyExchangeDemo.Services.Models
{
    public class GetExchangeRateResponseModel
    {
        public decimal SourceToTargetRate { get; set; }
        public decimal TargetToSourceRate { get; set; }
    }
}