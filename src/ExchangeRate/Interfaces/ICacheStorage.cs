using ExchangeRate.Providers.Models;

namespace ExchangeRate.Interfaces;

public interface ICacheStorage
{
    public void Save(IEnumerable<CurrencyPairRate> conversionRates);
    public IEnumerable<CurrencyPairRate> Load();
    public void Clear();
}