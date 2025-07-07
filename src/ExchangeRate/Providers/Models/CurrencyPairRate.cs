namespace ExchangeRate.Providers.Models;

/// <summary>
///     Represents a currency pair with its corresponding exchange rate.
/// </summary>
public record CurrencyPairRate(string FromCurrency, string ToCurrency, double Rate, DateTime UpdatedAt)
{
    public virtual bool Equals(CurrencyPairRate? obj)
    {
        if (obj == null) return false;

        return string.Equals(FromCurrency, obj.FromCurrency, StringComparison.Ordinal) &&
               string.Equals(ToCurrency, obj.ToCurrency, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FromCurrency, ToCurrency);
    }
}