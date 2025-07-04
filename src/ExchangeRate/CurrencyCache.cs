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

    public void SaveToCacheData(IEnumerable<ConversionData> conversionDataList)
    {
        foreach (var conversionData in conversionDataList)
        {
            if (!_cachedConversionData.ConversionData.Contains(conversionData))
            {
                _cachedConversionData.ConversionData.Add(conversionData);
            }
            else
            {
                UpdateCachedConversionData(conversionData);
            }
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

    private void UpdateCachedConversionData(ConversionData conversionData)
    {
        var oldConversionData = _cachedConversionData.ConversionData
            .FirstOrDefault(d => string.Equals(d.Key, conversionData.Key, StringComparison.Ordinal));

        if (oldConversionData is not null)
        {
            _cachedConversionData.ConversionData.Remove(oldConversionData);
        }

        _cachedConversionData.ConversionData.Add(conversionData);
    }
}