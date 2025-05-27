using System.Collections.Generic;
using System.Linq;
using CurrencyRateViewer.Models;

namespace CurrencyRateViewer.Services
{
    public class ConversionService
    {
        private readonly List<Conversion> _conversions = new();
        private int _nextId = 1;

        public List<Conversion> GetAll() => _conversions.OrderByDescending(c => c.Date).ToList();

        public Conversion GetById(int id) => _conversions.FirstOrDefault(c => c.Id == id);

        public void Add(Conversion conversion)
        {
            conversion.Id = _nextId++;
            _conversions.Add(conversion);
        }

        public void Update(Conversion updatedConversion)
        {
            var existing = GetById(updatedConversion.Id);
            if (existing != null)
            {
                existing.Amount = updatedConversion.Amount;
                existing.FromCurrency = updatedConversion.FromCurrency;
                existing.ToCurrency = updatedConversion.ToCurrency;
                existing.Result = updatedConversion.Result;
                existing.Date = updatedConversion.Date;
            }
        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null)
                _conversions.Remove(existing);
        }
    }
}
