namespace Bars.Gkh.Domain.DatabaseMutex
{
    using System;

    public class DatabaseMutexException : Exception
    {
        public DatabaseMutexException()
        {
            
        }

        public DatabaseMutexException(string message) : this(message, null)
        {
            
        }

        public DatabaseMutexException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}