using ExchangeRate.Cache.Interfaces;
using ExchangeRate.Providers.Models;

namespace ExchangeRate.Cache.Extensions;

public static class CurrencyCacheExtensions
{
    public static void SaveToCurrencyCache(this ICurrencyCache currencyCache, CurrencyPairRate currencyPairRate)
    {
        currencyCache.SaveToCurrencyCache([currencyPairRate]);
    }
}