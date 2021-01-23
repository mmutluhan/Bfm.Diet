using Castle.DynamicProxy;

namespace Bfm.Diet.Core.Interceptor
{
    public class LoggingInterceptor : IInterceptor
    {
        private readonly IAsyncInterceptor _asyncInterceptor;

        public LoggingInterceptor(LoggingInterceptorAsync asyncInterceptor)
        {
            _asyncInterceptor = asyncInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            _asyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}