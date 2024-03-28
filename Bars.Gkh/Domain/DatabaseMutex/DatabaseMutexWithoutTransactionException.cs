namespace Bars.Gkh.Domain.DatabaseMutex
{
    using System;

    public class DatabaseMutexWithoutTransactionException : ApplicationException
    {
        public DatabaseMutexWithoutTransactionException() : base("Attempt to acquire database mutex without active database transaction")
        {
        }
    }
}