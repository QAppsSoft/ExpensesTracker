using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace ExchangeRate;

public class CurrencyCache
{
    private readonly string _cachePath;
    private ConversionCacheDTO _cachedConversionData;

    public CurrencyCache(string cachePath)
    {
        _cachePath = cachePath;
        if (string.IsNullOrEmpty(cachePath))
        {
            throw new ArgumentNullException(nameof(cachePath));
        }

        var directoryName = Path.GetDirectoryName(_cachePath);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        if (!File.Exists(_cachePath))
        {
            var serializedConversionData = JsonSerializer.Serialize(ConversionCacheDTO.Default);
            File.WriteAllText(cachePath, serializedConversionData);
            _cachedConversionData = ConversionCacheDTO.Default;
        }
        else
        {
            var cacheText = File.ReadAllText(cachePath);
                
            var deserializedConversionData = JsonSerializer.Deserialize<ConversionCacheDTO>(cacheText);
            if (deserializedConversionData != null)
            {
                _cachedConversionData = deserializedConversionData;
            }
        }
    }

    [SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions",
        Justification = "Applying the fixes will not create more readable code")]
    public void SaveToCacheData(IEnumerable<ConversionData> conversionDataList)
    {
        foreach (var conversionData in conversionDataList)
        {
            if (_cachedConversionData.ConversionData.Add(conversionData))
            {
                continue;
            }
            
            _cachedConversionData.ConversionData.Remove(conversionData);
            _cachedConversionData.ConversionData.Add(conversionData);
        }

        SerializeAndSaveConversionData();
    }

    public void SaveToCacheData(ConversionData conversionData)
    {
        SaveToCacheData([conversionData]);
    }

    public ConversionData? GetCachedConversionData(string conversionKey) =>
        _cachedConversionData.ConversionData
            .FirstOrDefault(x => string.Equals(x.Key, conversionKey, StringComparison.Ordinal));

    public void ResetCache()
    {
        _cachedConversionData = ConversionCacheDTO.Default;
        SerializeAndSaveConversionData();
    }

    private void SerializeAndSaveConversionData()
    {
        var serializedConversionData = JsonSerializer.Serialize(_cachedConversionData);
        File.WriteAllText(_cachePath, serializedConversionData);
    }
}