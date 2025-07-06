using System.Globalization;
using System.Text.Json;
using ExchangeRate.Interfaces;
using ExchangeRate.Providers.Interfaces;
using ExchangeRate.Providers.Models;

namespace ExchangeRate.Providers;

public abstract class ProviderBase : IProvider, IDisposable
{
    protected readonly JsonSerializerOptions? Options = new() { PropertyNameCaseInsensitive = true };
    protected readonly HttpClient HttpClient;
    private const string CurrencyPairSeparator = "-";

    protected ProviderBase()
    {
        HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.Add("User-Agent", "ExchangeRateService");
        HttpClient.Timeout = TimeSpan.FromSeconds(30);
    }
    
    public abstract string Id { get; }
    public abstract string Name { get; }

    public async Task<IEnumerable<CurrencyPairRate>> GetRatesAsync(string baseCurrency)
    {
        var exchangeResponse = await GetAsync(baseCurrency).ConfigureAwait(false);

        var date = exchangeResponse.TimeLastUpdate;

        var rates = exchangeResponse.Rates.Select(rate =>
            new CurrencyPairRate(
                $"{baseCurrency.ToUpper(CultureInfo.InvariantCulture)}{CurrencyPairSeparator}{rate.Key.ToUpper(CultureInfo.InvariantCulture)}",
                rate.Value, date));

        return rates;
    }

    protected abstract Task<IExchangeRateResponse> GetAsync(string baseCurrency);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            HttpClient.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}