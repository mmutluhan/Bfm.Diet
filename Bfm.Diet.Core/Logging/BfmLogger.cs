using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Bfm.Diet.Core.Logging
{
    public class BfmLogger : ILogger
    {
        private readonly BfmLoggerProvider _bfmLoggerProvider;
        private string _category;

        public BfmLogger([NotNull] BfmLoggerProvider bfmLoggerProvider, string category)
        {
            _bfmLoggerProvider = bfmLoggerProvider;
            _category = category;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            //var logName = string.Concat("Log-", DateTimeOffset.UtcNow.ToString("yyyyMMdd"), ".log");
            //var fullFilePath = Path.Combine(Environment.CurrentDirectory, logName);

            var logRecord =
                $"{"[" + DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss+00:00") + "]"} [{logLevel.ToString()}] {formatter(state, exception)} {(exception != null ? exception.StackTrace : "")}";
            Debug.WriteLine(logRecord);
            //using (var streamWriter = new StreamWriter(fullFilePath, true, Encoding.UTF8))
            //{
            //    await streamWriter.WriteLineAsync(logRecord);
            //    streamWriter.Close();
            //    await streamWriter.DisposeAsync();
            //}
        }
    }
}