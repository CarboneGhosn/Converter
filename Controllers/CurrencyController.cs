using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CurrencyRateViewer.Services;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyRateViewer.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ExchangeRateService _rateService;

        public CurrencyController(ExchangeRateService rateService)
        {
            _rateService = rateService;
        }

        public async Task<IActionResult> Index()
        {
            var rates = await _rateService.GetRatesAsync() ?? new Dictionary<string, decimal>();
            ViewBag.BaseCurrency = "BRL"; // base sempre BRL
            return View(rates);
        }

        [HttpPost]
        public async Task<IActionResult> ConvertAjax(string fromCurrency, string toCurrency, decimal amount)
        {
            var rates = await _rateService.GetRatesAsync() ?? new Dictionary<string, decimal>();

            if (fromCurrency == toCurrency)
            {
                return Json(new { success = false, message = "Moeda de origem e destino não podem ser iguais." });
            }

            decimal fromRate = fromCurrency == "BRL" ? 1m : (rates.ContainsKey(fromCurrency) ? rates[fromCurrency] : 0m);
            decimal toRate = toCurrency == "BRL" ? 1m : (rates.ContainsKey(toCurrency) ? rates[toCurrency] : 0m);

            if (fromRate == 0m || toRate == 0m)
            {
                return Json(new { success = false, message = "Moeda inválida ou não disponível." });
            }

            decimal amountInBRL = amount * fromRate;
            decimal convertedAmount = amountInBRL / toRate;

            string resultMessage = $"{amount} {fromCurrency} = {convertedAmount:F4} {toCurrency}";

            return Json(new { success = true, message = resultMessage });
        }
    }
}
