using ExchangeRate.Providers.Models;

namespace ExchangeRate.Providers.Interfaces;

/// <summary>
///     Provides exchange rates for different currencies.
/// </summary>
public interface ICurrencyProvider
{
    /// <summary>
    ///     Gets a collection of providers that can be used to retrieve exchange rates.
    /// </summary>
    IEnumerable<IProvider> Providers { get; }

    /// <summary>
    ///     Retrieves a collection of exchange rates for the specified base currency.
    /// </summary>
    /// <param name="baseCurrency">The base currency to use when retrieving exchange rates.</param>
    /// <returns>A collection of exchange rate data.</returns>
    Task<IEnumerable<RateDto>> GetRatesAsync(string baseCurrency = "USD");

    /// <summary>
    ///     Retrieves a collection of exchange rates for the specified provider and base currency.
    /// </summary>
    /// <param name="provider">The provider to use when retrieving exchange rates.</param>
    /// <param name="baseCurrency">The base currency to use when retrieving exchange rates.</param>
    /// <returns>A collection of exchange rate data.</returns>
    Task<IEnumerable<RateDto>> GetRatesByProviderAsync(IProvider provider, string baseCurrency = "USD");
}