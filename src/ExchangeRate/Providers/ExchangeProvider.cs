using ExchangeRate.Providers.Interfaces;
using ExchangeRate.Providers.Models;

namespace ExchangeRate.Providers;

public class ExchangeProvider(IEnumerable<IProvider> providers) : IExchangeProvider
{
    public IEnumerable<IProvider> Providers { get; } = providers ?? throw new ArgumentNullException(nameof(providers));

    public Task<IEnumerable<RateDto>> GetRatesAsync(string baseCurrency = "USD")
    {
        return Providers.First().GetRatesAsync(baseCurrency);
    }

    public Task<IEnumerable<RateDto>> GetRatesByProviderAsync(IProvider provider, string baseCurrency = "USD")
    {
        return provider.GetRatesAsync(baseCurrency);
    }
}