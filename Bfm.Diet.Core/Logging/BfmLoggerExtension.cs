using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bfm.Diet.Core.Logging
{
    public static class BfmLoggerExtension
    {
        public static ILoggingBuilder BfmFileLogger(this ILoggingBuilder builder, Action<LoggingSettings> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, BfmLoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}