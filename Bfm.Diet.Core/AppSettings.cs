using System.Collections.Generic;
using Bfm.Diet.Core.Logging;
using Bfm.Diet.Core.Security.Jwt;

namespace Bfm.Diet.Core
{
    public class AppSettings
    {
        public TokenOptions TokenOptions { get; set; }
        public string SecretKey { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public LoggingSettings LoggingSettings { get; set; }
        public List<string> AllowedHosts { get; set; }
        public CacheSettings CacheSettings { get; set; }
    }

    public class CacheSettings
    {
        public string RedisConnection { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }
}