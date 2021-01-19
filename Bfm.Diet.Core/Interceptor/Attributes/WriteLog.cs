using Bfm.Diet.Core.Dependency;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bfm.Diet.Core.Interceptor.Attributes
{
    public class WriteLog : AttributeInterceptor
    {
        private ILogger<WriteLog> _logger;

        protected override void OnBefore(IInvocation invocation)
        {
            _logger = (ILogger<WriteLog>) ServiceResolver.ServiceProvider.GetRequiredService(typeof(ILogger<WriteLog>));
            _logger.LogInformation(MethodLog(invocation));
        }

        private string MethodLog(IInvocation invocation)
        {
            return $"{invocation.TargetType.FullName}.{invocation.Method.Name}()";
        }
    }
}