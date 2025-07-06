namespace ExchangeRate.Providers.Models;

/// <summary>
///     Represents a currency pair with its corresponding exchange rate.
/// </summary>
public record RateDto(string CurrencyPair, double Rate, DateTime LastUpdated);