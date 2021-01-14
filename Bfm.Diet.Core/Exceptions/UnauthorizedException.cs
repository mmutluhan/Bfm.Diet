using System;

namespace Bfm.Diet.Core.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string exceptionMessage) : base(exceptionMessage)
        {
            ExceptionMessage = exceptionMessage;
        }

        public UnauthorizedException(string exceptionMessage, string message) : base(message)
        {
            ExceptionMessage = exceptionMessage;
        }

        public UnauthorizedException(string exceptionMessage, string message, Exception innerException) :
            base(message,
                innerException)
        {
            ExceptionMessage = exceptionMessage;
        }

        public string ExceptionMessage { get; set; } = string.Empty;
    }
}