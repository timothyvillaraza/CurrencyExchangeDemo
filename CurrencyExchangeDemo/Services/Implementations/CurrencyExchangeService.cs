using CurrencyExchangeDemo.Services.Interfaces;
using CurrencyExchangeDemo.Services.Models;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;

namespace CurrencyExchangeDemo.Services.Implementations
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly HttpClient _httpClient;

        public CurrencyExchangeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // TODO: Create service that fetches valid currency selections, right now the currency select options are hard coded into the GetExchangeRateResponseModel
        public async Task<GetExchangeRateResponseModel> GetExchangeRateAsync(DateTime date, string sourceCurrencyName, string targetCurrencyName)
        {
            if (sourceCurrencyName.Equals(targetCurrencyName))
            {
                return new GetExchangeRateResponseModel()
                {
                    SourceToTargetRate = 1.00m,
                    TargetToSourceRate = 1.00m
                };
            }

            var apiResponse = await _httpClient.GetAsync($"https://cdn.jsdelivr.net/npm/@fawazahmed0/currency-api@{date.ToString("yyyy-MM-dd")}/v1/currencies/{sourceCurrencyName}.json");

            if (!apiResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch data: {apiResponse.StatusCode}");
            }

            // Deserialize JSON response directly
            var responseStream = await apiResponse.Content.ReadAsStreamAsync();
            var rawData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseStream);

            // Extract the date and rates
            string responseDate = rawData["date"].GetString();
            var rates = JsonSerializer.Deserialize<Dictionary<string, decimal>>(rawData[sourceCurrencyName].GetRawText());

            // TODO: Gracefully handle exception
            if (!rates.ContainsKey(targetCurrencyName))
            {
                throw new KeyNotFoundException($"Target currency '{targetCurrencyName}' not found.");
            }

            return new GetExchangeRateResponseModel()
            {
                SourceToTargetRate = rates[targetCurrencyName],
                TargetToSourceRate = 1 / rates[targetCurrencyName]
            };
        }
    }
}
