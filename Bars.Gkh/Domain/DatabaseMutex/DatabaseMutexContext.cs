namespace Bars.Gkh.Domain.DatabaseMutex
{
    using System;

    using Bars.B4.Application;

    public class DatabaseMutexContext : IDisposable
    {
        private readonly IDatabaseLockedMutexHandle handle;

        /// <summary>
        /// Блокировка действия на уровне БД через интерфейс у сущности 
        /// </summary>
        /// <param name="haveMutexName">Названия блокировки</param>
        /// <param name="description">Комментарий</param>
        public DatabaseMutexContext(IHaveMutexName haveMutexName, string description = null)
        {
            var mutexManager = ApplicationContext.Current.Container.Resolve<IDatabaseMutexManager>();

            if (!mutexManager.TryLock(haveMutexName, description, out this.handle))
            {
                throw new DatabaseMutexException("Не удалось получить блокировку");
            }
        }
        /// <summary>
        /// Блокировка действия на уровне БД 
        /// </summary>
        /// <param name="mutexName">Названия блокировки</param>
        /// <param name="description">Комментарий</param>
        public DatabaseMutexContext(string mutexName, string description = null)
        {
            var mutexManager = ApplicationContext.Current.Container.Resolve<IDatabaseMutexManager>();

            if (!mutexManager.TryLock(mutexName, description, out this.handle))
            {
                throw new DatabaseMutexException("Не удалось получить блокировку");
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.handle != null)
            {
                this.handle.Dispose();
            }
        }
    }
}