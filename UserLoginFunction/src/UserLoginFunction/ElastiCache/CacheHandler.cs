using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace UserLoginFunction.ElastiCache;

public class CacheHandler : ICacheHandler
{
    private readonly string? _elastiCachePrimaryEndpoint =
        Environment.GetEnvironmentVariable("ElastiCachePrimaryEndpoint");

    public async Task<bool> SaveStringToCache(string key, string input)
    {
        if (string.IsNullOrWhiteSpace(_elastiCachePrimaryEndpoint))
        {
            return false;
        }
        
        var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(_elastiCachePrimaryEndpoint);
        var redisDb = connectionMultiplexer.GetDatabase();
        
        return await redisDb.StringSetAsync(key, input, TimeSpan.FromHours(1));
    }
}