using System;
using System.Runtime.Serialization;

namespace GisGkhLibrary.Exceptions
{
    [Serializable]
    public class GISGKHRequestException : ApplicationException
    {
        public GISGKHRequestException()
        {
        }

        public GISGKHRequestException(string message) : base(message)
        {
        }

        public GISGKHRequestException(string message, Exception inner) : base(message, inner)
        {
        }

        protected GISGKHRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}