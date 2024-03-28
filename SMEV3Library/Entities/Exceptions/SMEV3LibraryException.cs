using System;
using System.Runtime.Serialization;

namespace SMEV3Library.Exceptions
{
    /// <summary>
    /// Исключение билиотеки
    /// </summary>
    public class SMEV3LibraryException : ApplicationException
    {
        public SMEV3LibraryException() { }

        public SMEV3LibraryException(string message) : base(message) { }

        public SMEV3LibraryException(string message, Exception inner) : base(message, inner) { }

        protected SMEV3LibraryException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
