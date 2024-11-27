using CurrencyExchangeDemo.Services.Models;

namespace CurrencyExchangeDemo.Services.Interfaces
{
    public interface ICurrencyExchangeService
    {
        public Task<GetExchangeRateResponseModel> GetExchangeRateAsync(DateTime date, string sourceCurrencyName, string targetCurrencyName);
    }
}
