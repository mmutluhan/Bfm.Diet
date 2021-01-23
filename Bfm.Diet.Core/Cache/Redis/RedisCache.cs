using System;
using System.Linq;
using System.Threading.Tasks;
using Bfm.Diet.Core.Cache.Base;
using Bfm.Diet.Core.Dependency;
using Bfm.Diet.Core.Json;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Bfm.Diet.Core.Cache.Redis
{
    public class RedisCache : CacheBase
    {
        private static Lazy<ConnectionMultiplexer> _redisConnection;
        private readonly string _redisConnectionString;
        private readonly bool _redisActive = true;
        private IOptions<AppSettings> _settings;

        public RedisCache(string name) : base(name)
        {
            try
            {
                _redisConnectionString = string.IsNullOrEmpty(Settings.Value.CacheSettings.RedisConnection)
                    ? "0.0.0.0:0000"
                    : Settings.Value.CacheSettings.RedisConnection;

                if (_redisConnection == null)
                    try
                    {
                        var configuration = ConfigurationOptions.Parse(_redisConnectionString, true);
                        configuration.ResolveDns = true;
                        _redisConnection =
                            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configuration));
                    }
                    catch (Exception)
                    {
                        _redisActive = false;
                    }
            }
            catch
            {
                throw new ApplicationException(_redisConnectionString +
                                               " Connection string format invalid <127.0.0.1:6379>");
            }
        }

        public IOptions<AppSettings> Settings
        {
            get
            {
                return _settings ??= (IOptions<AppSettings>) ServiceResolver.ServiceProvider.GetService(
                    typeof(IOptions<AppSettings>));
            }
        }

        private static ConnectionMultiplexer Connection => _redisConnection.Value;
        private static IDatabase Cache => Connection.GetDatabase();

        public override object GetOrDefault(string key)
        {
            try
            {
                if (!_redisActive)
                    return null;
                var data = Cache.StringGet(key);
                if (data.IsNullOrEmpty)
                    return null;
                var obj = JsonConvert.DeserializeObject(data.ToString(), SerializerSettings.BfmJsonSerializerSettings);
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override async Task<object> GetOrDefaultAsync(string key)
        {
            try
            {
                if (!_redisActive)
                    return await Task.FromResult(default(object)).ConfigureAwait(false);

                var data = await Cache.StringGetAsync(key);
                if (data.IsNullOrEmpty)
                    return await Task.FromResult(default(object)).ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject(data.ToString(), SerializerSettings.BfmJsonSerializerSettings);
                return await Task.FromResult(obj).ConfigureAwait(false);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null)
        {
            if (!_redisActive)
                return;
            Cache.StringSet(key, JsonConvert.SerializeObject(value, SerializerSettings.BfmJsonSerializerSettings),
                slidingExpireTime ?? DefaultSlidingExpireTime);
        }


        public override void Set<T>(string key, object value, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null)
        {
            if (!_redisActive)
                return;
            Cache.StringSet(key, JsonConvert.SerializeObject(value, SerializerSettings.BfmJsonSerializerSettings),
                slidingExpireTime ?? DefaultSlidingExpireTime);
        }

        public override T GetOrAdd<T>(string key, Func<T> operation, int lifetime, bool refresh)
        {
            if (!_redisActive)
                return default;

            var item = Get<T>(key, false);
            if (item == null || refresh)
            {
                item = operation.Invoke();
                var cacheItem = JsonConvert.SerializeObject(item, SerializerSettings.BfmJsonSerializerSettings);
                Set(key, cacheItem, TimeSpan.FromSeconds(lifetime));
                return item;
            }

            return item;
        }

        public override T Get<T>(string key, bool remove)
        {
            if (!_redisActive)
                return default;

            var item = GetOrDefault(key);
            if (item == null)
                return default;

            if (remove)
                Remove(key);

            return JsonConvert.DeserializeObject<T>(item.ToString(), SerializerSettings.BfmJsonSerializerSettings);
        }

        public override async Task<T> GetAsync<T>(string key, bool remove)
        {
            if (!_redisActive)
                return await Task.FromResult(default(T)).ConfigureAwait(false);

            var item = Get<T>(key, remove);
            if (item == null)
                return await Task.FromResult(default(T)).ConfigureAwait(false);

            if (remove)
                Remove(key);

            var obj = JsonConvert.DeserializeObject<T>(item.ToString(),
                SerializerSettings.BfmJsonSerializerSettings);
            return await Task.FromResult(obj).ConfigureAwait(false);
        }

        public override async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> operation, int lifetime, bool refresh)
        {
            if (!_redisActive)
                return await Task.FromResult(default(T)).ConfigureAwait(false);

            var item = Get<T>(key, false);
            if (item == null || refresh)
            {
                item = await operation.Invoke();
                var cacheItem = JsonConvert.SerializeObject(item, SerializerSettings.BfmJsonSerializerSettings);
                Set(key, cacheItem, TimeSpan.FromSeconds(lifetime));
                return await Task.FromResult(item).ConfigureAwait(false);
            }

            return await Task.FromResult(item).ConfigureAwait(false);
        }


        public override void Remove(string key)
        {
            Cache.KeyDelete(key);
        }

        public override void Clear()
        {
            var endpoints = Connection.GetEndPoints();
            var keys = Connection.GetServer(endpoints.First()).Keys();
            foreach (var key in keys)
            {
                Console.WriteLine("Removing Key {0} from cache", key.ToString());
                Cache.KeyDelete(key);
            }
        }
    }
}