using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Bfm.Diet.Core.Exceptions
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }


        public void HandleExceptions(Action act)
        {
            try
            {
                act();
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error ", ex);
                throw;
            }
        }

        public async Task HandleExceptions(Func<Task> task)
        {
            try
            {
                await task().ConfigureAwait(false);
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error ", ex);
                throw;
            }
        }

        public async Task<T> HandleExceptions<T>(Func<Task<T>> func)
        {
            try
            {
                return await func().ConfigureAwait(false);
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error ", ex);
                throw;
            }
        }
    }
}