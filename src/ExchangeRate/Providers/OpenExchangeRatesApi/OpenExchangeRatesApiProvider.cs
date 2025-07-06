using System.Text.Json;
using ExchangeRate.Exceptions;
using ExchangeRate.Interfaces;
using ExchangeRate.Providers.OpenExchangeRatesApi.Models;

namespace ExchangeRate.Providers.OpenExchangeRatesApi;

public class OpenExchangeRatesApiProvider(string appId) : ProviderBase
{
    private const string BaseUrl = "https://openexchangerates.org/api/";
    
    public override string Id { get; } = "open-exchange-rates-api";
    public override string Name { get; } = "Open Exchange Rates API";
    
    protected override async Task<IExchangeRateResponse> GetAsync(string baseCurrency)
    {
        try
        {
            var response = await HttpClient.GetAsync($"{BaseUrl}?app_id={appId}&base={baseCurrency}")
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new ExchangeRateApiException($"API request failed with status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var deserializedResponse = JsonSerializer.Deserialize<OpenExchangeRatesResponse>(content, Options);

            if (deserializedResponse is null)
            {
                throw new ExchangeRateApiException("Failed to deserialize exchange rate API response");
            }
            
            return deserializedResponse;
        }
        catch (HttpRequestException ex)
        {
            throw new ExchangeRateApiException("Network error while calling exchange rate API", ex);
        }
        catch (JsonException ex)
        {
            throw new ExchangeRateApiException("Error parsing exchange rate API response", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new ExchangeRateApiException("Request to exchange rate API timed out", ex);
        }
    }
}