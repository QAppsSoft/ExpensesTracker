using System.Text.Json;
using ExchangeRate.Exceptions;
using ExchangeRate.Interfaces;
using ExchangeRate.Providers.BrouApi.Models;

namespace ExchangeRate.Providers.BrouApi;

public class BrouProvider : ProviderBase
{
    private const string BaseUrl = "https://uruguayapi.onrender.com/api/v1/banks/brou_rates";
    
    public override string Id { get; } = "brou-api";
    public override string Name { get; } = "Brou API";
    protected override async Task<IExchangeRateResponse> GetAsync(string baseCurrency)
    {
        try
        {
            var response = await HttpClient.GetAsync(BaseUrl).ConfigureAwait(false);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new ExchangeRateApiException($"API request failed with status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var deserializedResponse = JsonSerializer.Deserialize<BrouRates>(content, Options);
            
            if (deserializedResponse is null)
            {
                throw new ExchangeRateApiException("Failed to deserialize exchange rate API response");
            }

            return new BrouResponse(deserializedResponse, DateTime.Now, baseCurrency);
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