using ExchangeRate.Cache.Interfaces;
using ExchangeRate.Interfaces;
using ExchangeRate.Providers.Interfaces;

namespace ExchangeRate;

public class CurrencyConversionProvider(IExchangeProvider exchangeProvider, ICurrencyCache currencyCache)
{
    public async Task<double> ConvertAsync(double amount, string fromCurrency, string toCurrency)
    {
        // Check if the currency rate is already cached and return the result if it is.
        var cached = currencyCache.GetCachedConversionData(fromCurrency, toCurrency);
        if (cached is not null)
        {
            return amount * cached.Rate;
        }

        var rates = await exchangeProvider.GetRatesAsync(fromCurrency).ConfigureAwait(false);

        var rate = rates.FirstOrDefault(r =>
            string.Equals(r.FromCurrency, fromCurrency, StringComparison.Ordinal) &&
            string.Equals(r.ToCurrency, toCurrency, StringComparison.Ordinal));

        if (rate is null)
        {
            throw new InvalidOperationException($"Rate for {fromCurrency}-{toCurrency} not found.");
        }

        currencyCache.SaveToCacheData(rate);
        
        return amount * rate.Rate;
    }
}