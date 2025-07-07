using ExchangeRate.Providers.Models;

namespace ExchangeRate.Cache.Interfaces;

public interface ICurrencyCache
{
    void SaveToCacheData(IEnumerable<CurrencyPairRate> conversionDataList);
    CurrencyPairRate? GetCachedConversionData(string fromCurrency, string toCurrency);
    void ResetCache(bool onlyExpired = false);
}