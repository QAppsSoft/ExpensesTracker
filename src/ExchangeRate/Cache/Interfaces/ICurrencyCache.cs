using ExchangeRate.Providers.Models;

namespace ExchangeRate.Cache.Interfaces;

public interface ICurrencyCache
{
    void SaveToCurrencyCache(IEnumerable<CurrencyPairRate> currencyPairs);
    CurrencyPairRate? LoadFromCurrencyCache(string fromCurrency, string toCurrency);
    void ClearCache(bool onlyExpired = false);
}