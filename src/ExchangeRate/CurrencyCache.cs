using ExchangeRate.Providers.Models;

namespace ExchangeRate;

public class CurrencyCache
{
    private readonly CacheStorage _cacheStorage;
    private HashSet<CurrencyPairRate> _cache;
    public static readonly TimeSpan CacheDuration = TimeSpan.FromHours(12);

    public CurrencyCache(CacheStorage cacheStorage)
    {
        _cacheStorage = cacheStorage ?? throw new ArgumentNullException(nameof(cacheStorage));
        _cache = _cacheStorage.Load().ToHashSet();
    }
    
    public void SaveToCacheData(IEnumerable<CurrencyPairRate> conversionDataList)
    {
        var oldCache = _cache;
        
        var combinedSet = conversionDataList.ToHashSet();
        combinedSet.UnionWith(oldCache);

        _cache = combinedSet;
        
        _cacheStorage.Save(_cache);
    }

    public void SaveToCacheData(CurrencyPairRate conversionData)
    {
        SaveToCacheData([conversionData]);
    }

    public CurrencyPairRate? GetCachedConversionData(string fromCurrency, string toCurrency) =>
        _cache.FirstOrDefault(x =>
            string.Equals(x.FromCurrency, fromCurrency, StringComparison.Ordinal) &&
            string.Equals(x.ToCurrency, toCurrency, StringComparison.Ordinal) &&
            !IsExpired(x));

    public void ResetCache(bool onlyExpired = false)
    {
        if (onlyExpired)
        {
            var expiredCache = _cache.Where(IsExpired);

            _cache = _cache.Except(expiredCache).ToHashSet();
        }
        else
        {
            _cache = [];
        }
        
        _cacheStorage.Save(_cache);
    }

    private static bool IsExpired(CurrencyPairRate conversionData)
    {
        return conversionData.UpdatedAt < DateTime.Now - CacheDuration;
    }
}