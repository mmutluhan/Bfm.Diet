using System;

namespace Bfm.Diet.Core.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string exceptionMessage) : base(exceptionMessage)
        {
            ExceptionMessage = exceptionMessage;
        }

        public NotFoundException(string exceptionMessage, string message) : base(message)
        {
            ExceptionMessage = exceptionMessage;
        }

        public NotFoundException(string exceptionMessage, string message, Exception innerException) : base(message,
            innerException)
        {
            ExceptionMessage = exceptionMessage;
        }

        public string ExceptionMessage { get; set; } = string.Empty;
    }
}