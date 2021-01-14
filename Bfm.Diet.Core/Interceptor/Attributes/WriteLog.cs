using System;
using System.Linq.Expressions;
using Bfm.Diet.Core.Dependency;
using Bfm.Diet.Core.Json;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serialize.Linq.Serializers;
using JsonSerializer = Serialize.Linq.Serializers.JsonSerializer;

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