using System.Text.Json;
using HR_Management.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;

namespace HR_Management.Infrastructure.Cache;

public class RedisCacheService : ICacheService
{
    // The underlying distributed cache (Redis, SQL Server, In-Memory, etc.)
    private readonly IDistributedCache _cache;
    // Default expiry: 30 min absolute, 5 min sliding
    private readonly DistributedCacheEntryOptions _defaultOptions;

    // JSON serializer settings — case-insensitive so property names can differ in casing
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
        _defaultOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
            SlidingExpiration = TimeSpan.FromMinutes(5)
        };
    }

    // Read from cache and deserialize. Returns null if key doesn't exist.
    public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var bytes = await _cache.GetAsync(key, cancellationToken);
        if (bytes is null) return null!;
        return JsonSerializer.Deserialize<T>(bytes, JsonOptions)!;
    }

    // Serialize and store. Uses custom expiry if provided, otherwise defaults.
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T : class
    {
        var options = _defaultOptions;
        if (expiry.HasValue)
        {
            options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry
            };
        }

        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions);
        await _cache.SetAsync(key, bytes, options, cancellationToken);
    }

    // Remove a key from cache
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }
}