using ExchangeRate.Providers.Models;

namespace ExchangeRate.Interfaces;

public interface ICurrencyCache
{
    void SaveToCacheData(IEnumerable<CurrencyPairRate> conversionDataList);
    void SaveToCacheData(CurrencyPairRate conversionData);
    CurrencyPairRate? GetCachedConversionData(string fromCurrency, string toCurrency);
    void ResetCache(bool onlyExpired = false);
}