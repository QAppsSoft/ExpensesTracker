using System.Text.Json;
using ExchangeRate.Interfaces;

namespace ExchangeRate;

public class CacheStorage : ICacheStorage
{
    private readonly string _cachePath;

    public CacheStorage(string cachePath)
    {
        if (string.IsNullOrWhiteSpace(cachePath))
        {
            throw new ArgumentNullException(nameof(cachePath));
        }

        _cachePath = cachePath;
    }

    public void Save(IEnumerable<ConversionData> conversionRates)
    {
        var directoryName = Path.GetDirectoryName(_cachePath);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
        
        var serializedConversionData = JsonSerializer.Serialize(conversionRates);
        File.WriteAllText(_cachePath, serializedConversionData);
    }

    public IEnumerable<ConversionData> Load()
    {
        if (!File.Exists(_cachePath))
        {
            return [];
        }
        
        var serializedConversionRates = File.ReadAllText(_cachePath);
        var conversionRates = JsonSerializer.Deserialize<IEnumerable<ConversionData>>(serializedConversionRates);
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