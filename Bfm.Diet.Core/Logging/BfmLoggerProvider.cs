using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bfm.Diet.Core.Logging
{
    public class BfmLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, BfmLogger> _loggers =
            new ConcurrentDictionary<string, BfmLogger>();

        private IOptions<AppSettings> _options;

        public BfmLoggerProvider(IOptions<AppSettings> options)
        {
            _options = options;
        }

        public LoggingSettings LoggingSettings { get; private set; }

        public void Dispose()
        {
            _loggers.Clear();
        }

        ILogger ILoggerProvider.CreateLogger(string category)
        {
            return _loggers.GetOrAdd(category, category => { return new BfmLogger(this, category); });
        }
    }
}