using System;
using System.Runtime.Serialization;

namespace Bfm.Diet.Core.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException()
        {
        }


        public ApplicationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }


        public ApplicationException(string message)
            : base(message)
        {
        }


        public ApplicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}