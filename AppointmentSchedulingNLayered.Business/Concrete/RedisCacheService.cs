using AppointmentSchedulingNLayered.Business.Abstract;
using AppointmentSchedulingNLayered.Common.Jwt;
using AppointmentSchedulingNLayered.Common.Redis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace AppointmentSchedulingNLayered.Business.Concrete;
public class RedisCacheService : ICacheService {
    private IDatabase _cacheDb;
    private RedisSettings _redisSettings;

    public RedisCacheService(IOptions<RedisSettings> redisSettings) {
        _redisSettings = redisSettings.Value;
        var redis = ConnectionMultiplexer.Connect(_redisSettings.Connection);
        _cacheDb = redis.GetDatabase();
    }

    public T GetData<T>(string key) {
        var value = _cacheDb.StringGet(key);
        if (!string.IsNullOrEmpty(value))
            return JsonSerializer.Deserialize<T>(value);

        return default;
    }

    public object RemoveData(string key) {
        var exists = _cacheDb.KeyExists(key);
        if (exists)
            return _cacheDb.KeyDelete(key);

        return false;
    }

    public bool SetData<T>(string key, T value) {
        var expiryTime = DateTimeOffset.UtcNow.AddSeconds(_redisSettings.ExpiryTimeInSeconds).DateTime.Subtract(DateTime.UtcNow);
        return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
    }
}