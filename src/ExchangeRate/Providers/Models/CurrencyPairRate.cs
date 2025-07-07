namespace ExchangeRate.Providers.Models;

/// <summary>
///     Represents a currency pair with its corresponding exchange rate.
/// </summary>
public record CurrencyPairRate(string CurrencyPair, double Rate, DateTime UpdatedAt);