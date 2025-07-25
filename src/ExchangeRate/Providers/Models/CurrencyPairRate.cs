namespace ExchangeRate.Providers.Models;

/// <summary>
///     Represents a currency pair with its corresponding exchange rate.
/// </summary>
public record CurrencyPairRate(string FromCurrency, string ToCurrency, decimal Rate, DateTime UpdatedAt);