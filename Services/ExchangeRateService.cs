using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;

namespace CurrencyRateViewer.Services
{
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Dictionary<string, decimal>> GetRatesAsync()
        {
            var url = "https://economia.awesomeapi.com.br/json/last/USD-BRL,EUR-BRL,GBP-BRL,JPY-BRL,AUD-BRL,CAD-BRL";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            using var json = JsonDocument.Parse(content);

            var result = new Dictionary<string, decimal>();

            foreach (var property in json.RootElement.EnumerateObject())
            {
                var item = property.Value;
                var code = item.GetProperty("code").GetString();
                var bidString = item.GetProperty("bid").GetString();

                if (decimal.TryParse(bidString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal bid))
                {
                    result[code] = bid;
                }
            }

            return result;
        }
    }
}
