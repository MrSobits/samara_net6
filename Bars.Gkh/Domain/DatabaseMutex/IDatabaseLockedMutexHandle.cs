namespace Bars.Gkh.Domain.DatabaseMutex
{
    using System;

    public interface IDatabaseLockedMutexHandle : IDisposable
    {
        /// <summary>
        /// Имя мьютекса
        /// </summary>
        string MutexName { get; set; }
    }
}