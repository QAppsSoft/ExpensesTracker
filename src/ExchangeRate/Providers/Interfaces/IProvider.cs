using ExchangeRate.Providers.Models;

namespace ExchangeRate.Providers.Interfaces;

/// <summary>
///     Provides a standardized interface for currency exchange rate providers.
/// </summary>
public interface IProvider
{
    /// <summary>
    ///     Unique identifier for this provider instance.
    /// </summary>
    string Id { get; }

    /// <summary>
    ///     Human-readable name of the provider.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Retrieves a collection of currency exchange rates for the specified base currency.
    /// </summary>
    /// <param name="baseCurrency">The base currency to retrieve exchange rates for.</param>
    /// <returns>A sequence of RateDto objects representing the exchange rates.</returns>
    Task<IEnumerable<CurrencyPairRate>> GetRatesAsync(string baseCurrency);
}