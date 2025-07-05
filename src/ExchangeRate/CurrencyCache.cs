using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace ExchangeRate;

public class CurrencyCache
{
    private readonly string _cachePath;
    private ConversionCacheDTO _cachedConversionData;
    public static readonly TimeSpan CacheDuration = TimeSpan.FromHours(12);

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
        var oldConversionData = _cachedConversionData.ConversionData;
        
        var combinedSet = conversionDataList.ToHashSet();
        combinedSet.UnionWith(oldConversionData);

        _cachedConversionData = new ConversionCacheDTO(DateTime.Now, combinedSet);
        
        SerializeAndSaveConversionData();
    }

    public void SaveToCacheData(ConversionData conversionData)
    {
        SaveToCacheData([conversionData]);
    }

    public ConversionData? GetCachedConversionData(string conversionKey) =>
        _cachedConversionData.ConversionData
            .FirstOrDefault(x =>
                string.Equals(x.Key, conversionKey, StringComparison.Ordinal) &&
                !IsExpired(x));

    public void ResetCache(bool onlyExpired = false)
    {
        if (onlyExpired)
        {
            var expiredCache = _cachedConversionData.ConversionData.Where(IsExpired);

            _cachedConversionData = new ConversionCacheDTO(DateTime.Now,
                _cachedConversionData.ConversionData.Except(expiredCache).ToHashSet());
        }
        else
        {
            _cachedConversionData = ConversionCacheDTO.Default;
        }
        
        SerializeAndSaveConversionData();
    }

    private void SerializeAndSaveConversionData()
    {
        var serializedConversionData = JsonSerializer.Serialize(_cachedConversionData);
        File.WriteAllText(_cachePath, serializedConversionData);
    }

    private static bool IsExpired(ConversionData conversionData)
    {
        return conversionData.UpdatedAt < DateTime.Now - CacheDuration;
    }
}