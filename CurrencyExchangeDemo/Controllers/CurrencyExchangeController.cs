using System.Diagnostics;
using CurrencyExchangeDemo.Models;
using CurrencyExchangeDemo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchangeDemo.Controllers
{
    public class CurrencyExchangeController : Controller
    {
        private readonly ILogger<CurrencyExchangeController> _logger;
        private readonly ICurrencyExchangeService _currencyExchangeService;

        public CurrencyExchangeController(ILogger<CurrencyExchangeController> logger
            , ICurrencyExchangeService currencyExchangeService)
        {
            _logger = logger;
            _currencyExchangeService = currencyExchangeService;
        }

        [HttpGet]
        public IActionResult CurrencyExchange()
        {
            // TODO: API Call to get valid currency drop downs

            // Starting form values (NOTE: Converstion Rates are only fetched from the 3rd Party API after both currencies are set, not in the initial GET of the page)
            var model = new CurrencyExchangeViewModel
            {
                SourceCurrencyName = "USD",
                TargetCurrencyName = "USD",
                Date = DateTime.Today,
                SourceToTargetRate = 1.00m,
                TargetToSourceRate = 1.00m
            };

            return View(model);
        }

        // UNUSED: For form submission
        [HttpPost]
        public async Task<IActionResult> CurrencyExchange(CurrencyExchangeViewModel viewModel)
        {
            // Form Validation
            if (!ModelState.IsValid)
            {
                // Return the form with validation messages
                return View(viewModel);
            }

            // External Third-Party API Call
            var getExchangeRateResponse = await _currencyExchangeService.GetExchangeRateAsync(
                viewModel.Date,
                viewModel.SourceCurrencyName,
                viewModel.TargetCurrencyName
            );

            // Mapping
            viewModel.SourceToTargetRate = getExchangeRateResponse.SourceToTargetRate;
            viewModel.TargetToSourceRate = getExchangeRateResponse.TargetToSourceRate;

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
