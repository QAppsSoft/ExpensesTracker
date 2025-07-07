using ExchangeRate.Providers.Interfaces;

namespace ExchangeRate.Providers;

/// <summary>
///     Implementation of <see cref="IProviderSelector" /> that selects the first provider in the list.
/// </summary>
public class FirstProviderSelector : IProviderSelector
{
    /// <summary>
    ///     Selects the first provider in the list.
    /// </summary>
    /// <param name="providers"></param>
    /// <returns>The first provider in the list.</returns>
    public IProvider SelectProvider(IEnumerable<IProvider> providers)
    {
        return providers.First();
    }
}