using System.Diagnostics;
using CurrencyExchangeDemo.Controllers.Requests;
using CurrencyExchangeDemo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchangeDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyExchangeApiController : ControllerBase
    {
        private readonly ILogger<CurrencyExchangeApiController> _logger;
        private readonly ICurrencyExchangeService _currencyExchangeService;

        public CurrencyExchangeApiController(
            ILogger<CurrencyExchangeApiController> logger,
            ICurrencyExchangeService currencyExchangeService)
        {
            _logger = logger;
            _currencyExchangeService = currencyExchangeService;
        }

        // Called by currency drop down selections
        [HttpPost("GetConversionRates")]
        public async Task<IActionResult> GetConversionRates(ConversionRateRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { Error = "Invalid request." });
            }

            // Request Validation
            if (request.Date > DateTime.Now)
            {
                return BadRequest(new { Error = "Date cannot be in the future." });
            }

            var getExchangeRateResponse = await _currencyExchangeService.GetExchangeRateAsync(
                request.Date,
                request.SourceCurrencyName,
                request.TargetCurrencyName
            );

            if (getExchangeRateResponse == null)
            {
                return NotFound(new { Error = "Exchange rate not found." });
            }

            return Ok(getExchangeRateResponse);
        }
    }
}

// NOTE ABOUT ROUTING: [Route] above the class configures routing.
// API Path: <root>/api/[controller]/[action]
// [controller]: Class Name w/o "Controller"
// [action]: Method Name, if included, we would just use [HttpPost] above each method as it would take from the method name.