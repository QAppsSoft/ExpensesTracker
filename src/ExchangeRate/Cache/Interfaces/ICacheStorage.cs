using ExchangeRate.Providers.Models;

namespace ExchangeRate.Cache.Interfaces;

public interface ICacheStorage
{
    public void Save(IEnumerable<CurrencyPairRate> conversionRates);
    public IEnumerable<CurrencyPairRate> Load();
    public void Clear();
}