namespace ExchangeRate.Providers.Interfaces;

/// <summary>
///     Interface for selecting a provider from a list of providers.
/// </summary>
public interface IProviderSelector
{
    /// <summary>
    ///     Selects a provider from the given list of providers.
    /// </summary>
    /// <param name="providers">The list of providers to select from.</param>
    /// <returns>The selected provider.</returns>
    IProvider SelectProvider(IEnumerable<IProvider> providers);
}