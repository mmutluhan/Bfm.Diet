using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bfm.Diet.Core.Attributes;
using Castle.DynamicProxy; 
using Serilog; 

namespace Bfm.Diet.Core.Interceptor
{
    public class LoggingInterceptorAsync : IAsyncInterceptor
    {
        protected static readonly List<Func<IInvocation, BaseAttribute>> AttributeSelectors =
            new List<Func<IInvocation, BaseAttribute>>
            {
                invocation => invocation.MethodInvocationTarget.GetCustomAttribute<LogAttribute>(),
                invocation => invocation.TargetType.GetCustomAttribute<LogAttribute>(),
                invocation => invocation.Method.GetCustomAttribute<LogAttribute>(),
                invocation => invocation.Method.DeclaringType?.GetCustomAttribute<LogAttribute>()
            };

        private readonly ILogger _logger;

        public LoggingInterceptorAsync()
        {
            _logger =  Log.ForContext<LoggingInterceptorAsync>();;
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            if (!CanIntercept(invocation))
            {
                invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
                return;
            }

            _logger.Information(FormatLogMessage(invocation, "start"));
            try
            {
                invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
            }
            finally
            {
                _logger.Information(FormatLogMessage(invocation, "end"));
            }
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            if (!CanIntercept(invocation))
            {
                invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
                return;
            }

            _logger.Information(FormatLogMessage(invocation, "start"));
            try
            {
                invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
            }
            finally
            {
                _logger.Information(FormatLogMessage(invocation, "end"));
            }
        }

        public void InterceptSynchronous(IInvocation invocation)
        {
            if (!CanIntercept(invocation))
            {
                invocation.Proceed();
                return;
            }

            _logger.Information(FormatLogMessage(invocation, "start"));
            try
            {
                invocation.Proceed();
            }
            finally
            {
                _logger.Information(FormatLogMessage(invocation, "end"));
            }
        }

        protected static BaseAttribute HasLogAttribute(IInvocation invocation)
        {
            return AttributeSelectors.Select(selector => selector(invocation))
                .FirstOrDefault(attribute => attribute != null);
        }

        private static bool CanIntercept(IInvocation invocation)
        {
            var attribute = HasLogAttribute(invocation);
            return attribute != null && attribute.Enabled;
        }

        private async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            invocation.Proceed();
            var task = (Task) invocation.ReturnValue;
            await task;
        }

        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.Proceed();
            var task = (Task<TResult>) invocation.ReturnValue;
            var result = await task;
            return result;
        }

        private string FormatLogMessage(IInvocation invocation, string message)
        {
            return $"{invocation.TargetType.FullName}#{invocation.MethodInvocationTarget.Name}  {message}";
        }
    }
}