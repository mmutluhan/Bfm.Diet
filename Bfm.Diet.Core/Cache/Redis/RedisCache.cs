using System;
using System.Linq;
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

        private IOptions<AppSettings> _settings;

        public RedisCache(string name) : base(name)
        {
            try
            {
                _redisConnectionString = string.IsNullOrEmpty(Settings.Value.CacheSettings.RedisConnection)
                    ? "0.0.0.0:0000"
                    : Settings.Value.CacheSettings.RedisConnection;

                if (_redisConnection == null)
                {
                    var configuration = ConfigurationOptions.Parse(_redisConnectionString, true);
                    configuration.ResolveDns = true;
                    _redisConnection =
                        new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configuration));
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
            var data = Cache.StringGet(key);
            if (data.IsNullOrEmpty)
                return null;
            return JsonConvert.DeserializeObject(data.ToString(), SerializerSettings.BfmJsonSerializerSettings);
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null)
        {
            Cache.StringSet(key, JsonConvert.SerializeObject(value, SerializerSettings.BfmJsonSerializerSettings), TimeSpan.MaxValue);
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