using System;
using System.Runtime.Serialization;

namespace GisGkhLibrary.Exceptions
{
    [Serializable]
    public class GISGKHAnswerException : ApplicationException
    {
        public GISGKHAnswerException()
        {
        }

        public GISGKHAnswerException(string message) : base(message)
        {
        }

        public GISGKHAnswerException(string message, Exception inner) : base(message, inner)
        {
        }

        protected GISGKHAnswerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}