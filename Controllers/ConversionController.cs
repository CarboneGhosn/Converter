using Microsoft.AspNetCore.Mvc;
using CurrencyRateViewer.Models;
using CurrencyRateViewer.Services;
using System;
using System.Threading.Tasks;

namespace CurrencyRateViewer.Controllers
{
    public class ConversionController : Controller
    {
        private readonly ConversionService _conversionService;
        private readonly ExchangeRateService _exchangeRateService;

        public ConversionController(ConversionService conversionService, ExchangeRateService exchangeRateService)
        {
            _conversionService = conversionService;
            _exchangeRateService = exchangeRateService;
        }

        // Lista conversões
        public IActionResult Index()
        {
            var conversions = _conversionService.GetAll();
            return View(conversions);
        }

        // Formulário para nova conversão
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Recebe formulário e faz conversão
        [HttpPost]
        public async Task<IActionResult> Create(decimal amount, string fromCurrency, string toCurrency)
        {
            if (amount <= 0 || string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency))
            {
                ModelState.AddModelError("", "Preencha todos os campos corretamente.");
                return View();
            }

            var rates = await _exchangeRateService.GetRatesAsync();

            // Se API não carregou as taxas
            if (rates == null)
            {
                ModelState.AddModelError("", "Não foi possível carregar as taxas.");
                return View();
            }

            if (!rates.ContainsKey(fromCurrency) || !rates.ContainsKey(toCurrency))
            {
                ModelState.AddModelError("", "Moeda inválida.");
                return View();
            }

            // Convertendo amount from "fromCurrency" to "toCurrency"
            // Lembre-se: rates são base BRL, então vamos converter via BRL
            decimal amountInBRL = amount / rates[fromCurrency]; // valor em BRL
            decimal convertedAmount = amountInBRL * rates[toCurrency]; // convertido na moeda desejada

            var conversion = new Conversion
            {
                Amount = amount,
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                Result = Math.Round(convertedAmount, 4),
                Date = DateTime.Now
            };

            _conversionService.Add(conversion);

            return RedirectToAction("Details", new { id = conversion.Id });
        }

        // Visualizar resultado
        public IActionResult Details(int id)
        {
            var conversion = _conversionService.GetById(id);
            if (conversion == null) return NotFound();

            return View(conversion);
        }

        // Editar (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var conversion = _conversionService.GetById(id);
            if (conversion == null) return NotFound();

            return View(conversion);
        }

        // Editar (POST)
        [HttpPost]
        public IActionResult Edit(Conversion conversion)
        {
            if (!ModelState.IsValid)
                return View(conversion);

            _conversionService.Update(conversion);
            return RedirectToAction("Index");
        }

        // Excluir (GET)
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var conversion = _conversionService.GetById(id);
            if (conversion == null) return NotFound();

            return View(conversion);
        }

        // Excluir (POST)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _conversionService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
