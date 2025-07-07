namespace ExchangeRate.Providers.Models;

public class CurrencyPairRateComparer : IEqualityComparer<CurrencyPairRate>
{
    public static CurrencyPairRateComparer CreateInstance()
    {
        return new CurrencyPairRateComparer();
    }

    public bool Equals(CurrencyPairRate? x, CurrencyPairRate? y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        return string.Equals(x.FromCurrency, y.FromCurrency, StringComparison.Ordinal) &&
               string.Equals(x.ToCurrency, y.ToCurrency, StringComparison.Ordinal);
    }

    public int GetHashCode(CurrencyPairRate obj)
    {
        return HashCode.Combine(obj.FromCurrency, obj.ToCurrency);
    }
}