using System;
using System.Runtime.Serialization;

namespace GisGkhLibrary.Xades.Exceptions
{
    [Serializable]
    public class XadesBesValidationException : Exception
    {
        public XadesBesValidationException()
        {
        }

        public XadesBesValidationException(string message) : base(message)
        {
        }

        public XadesBesValidationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected XadesBesValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}