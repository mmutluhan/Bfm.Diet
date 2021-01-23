using System;
using System.Threading.Tasks;

namespace Bfm.Diet.Core.Exceptions
{
    public interface IExceptionHandler
    {
        void HandleExceptions(Action act);
        Task HandleExceptions(Func<Task> task);
        Task<T> HandleExceptions<T>(Func<Task<T>> task);
    }
}