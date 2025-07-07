using ExchangeRate.Providers.Interfaces;
using ExchangeRate.Providers.Models;

namespace ExchangeRate.Providers;

/// <summary>
///    Provides exchange rates from multiple providers.
/// </summary>
/// <param name="providers">The list of providers to use.</param>
/// <param name="providerSelector">The selector to use for selecting a provider.</param>
public class ExchangeProvider(IEnumerable<IProvider> providers, IProviderSelector providerSelector) : IExchangeProvider
{
    /// <summary>
    ///   Gets the list of providers.
    /// </summary>
    public IEnumerable<IProvider> Providers { get; } = providers ?? throw new ArgumentNullException(nameof(providers));

    /// <summary>
    ///  Gets the provider selector.
    /// </summary>
    /// <param name="baseCurrency">The base currency to get rates for.</param>
    /// <returns>The exchange rates.</returns>
    public Task<IEnumerable<CurrencyPairRate>> GetRatesAsync(string baseCurrency = "USD")
    {
        return providerSelector.SelectProvider(Providers).GetRatesAsync(baseCurrency);
    }

    /// <summary>
    ///    Gets the exchange rates from the specified provider.
    /// </summary>
    /// <param name="provider">The provider to get rates from.</param>
    /// <param name="baseCurrency">The base currency to get rates for.</param>
    /// <returns></returns>
    public Task<IEnumerable<CurrencyPairRate>> GetRatesByProviderAsync(IProvider provider, string baseCurrency = "USD")
    {
        return provider.GetRatesAsync(baseCurrency);
    }
}