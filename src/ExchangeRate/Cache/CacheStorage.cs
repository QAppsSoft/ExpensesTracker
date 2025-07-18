using System.Text.Json;
using ExchangeRate.Cache.Interfaces;
using ExchangeRate.Providers.Models;

namespace ExchangeRate.Cache;

public class CacheStorage : ICacheStorage
{
    private readonly string _cachePath;

    public CacheStorage(string cachePath)
    {
        if (string.IsNullOrWhiteSpace(cachePath))
        {
            throw new ArgumentNullException(nameof(cachePath));
        }
        
        var directoryName = Path.GetDirectoryName(cachePath);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        _cachePath = cachePath;
    }

    public void Save(IEnumerable<CurrencyPairRate> conversionRates)
    {
        var serializedConversionData = JsonSerializer.Serialize(conversionRates);
        File.WriteAllText(_cachePath, serializedConversionData);
    }

    public IEnumerable<CurrencyPairRate> Load()
    {
        if (!File.Exists(_cachePath))
        {
            return [];
        }
        
        var serializedConversionRates = File.ReadAllText(_cachePath);
        var conversionRates = JsonSerializer.Deserialize<IEnumerable<CurrencyPairRate>>(serializedConversionRates);
        return conversionRates ?? [];
    }

    public void Clear()
    {
        if (File.Exists(_cachePath))
        {
            File.Delete(_cachePath);
        }
    }
}