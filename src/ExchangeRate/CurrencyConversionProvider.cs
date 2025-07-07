using ExchangeRate.Providers.Interfaces;

namespace ExchangeRate;

public class CurrencyConversionProvider(IExchangeProvider exchangeProvider)
{
    public async Task<double> ConvertAsync(double amount, string fromCurrency, string toCurrency)
    {
        var rates = await exchangeProvider.GetRatesAsync(fromCurrency).ConfigureAwait(false);

        var rate = rates.FirstOrDefault(r =>
            string.Equals(r.FromCurrency, fromCurrency, StringComparison.Ordinal) &&
            string.Equals(r.ToCurrency, toCurrency, StringComparison.Ordinal));

        if (rate is null)
        {
            throw new InvalidOperationException($"Rate for {fromCurrency}-{toCurrency} not found.");
        }

        return amount * rate.Rate;
    }
}