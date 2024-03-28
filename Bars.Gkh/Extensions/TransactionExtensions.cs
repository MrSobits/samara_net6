namespace Bars.Gkh.Extensions
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.Gkh.Domain.DatabaseMutex;

    using Castle.MicroKernel.Lifestyle;
    using Castle.MicroKernel.Lifestyle.Scoped;
    using Castle.Windsor;

    /// <summary>Вспомогательные методы для работы с блокировками</summary>
    public static class TransactionExtensions
    {
        /// <summary>Попробовать установить блокировку для операции <paramref name="mutexName"/> с открытием транзакции></summary>
        /// <param name="container">IWindsorContainer</param>
        /// <param name="mutexName">Имя операции</param>
        /// <param name="mutexDescription">Описание операции</param>
        /// <param name="action">Метод на выполнение</param>
        public static void InTransactionWithMutexLock(this IWindsorContainer container, string mutexName, string mutexDescription, Action action)
        {
            container.InTransaction(() => container.InMutexLock(mutexName, mutexDescription, action));
        }

        /// <summary>Попробовать установить блокировку для операции <paramref name="mutexName"/>></summary>
        /// <param name="container">IWindsorContainer</param>
        /// <param name="mutexName">Имя операции</param>
        /// <param name="mutexDescription">Описание операции</param>
        /// <param name="action">Метод на выполнение</param>
        public static void InMutexLock(this IWindsorContainer container, string mutexName, string mutexDescription, Action action)
        {
            IDatabaseLockedMutexHandle mutexHandle = null;
            try
            {
                var databaseMutexManager = container.Resolve<IDatabaseMutexManager>();

                mutexHandle = databaseMutexManager.Lock(mutexName, mutexDescription);

                action();
            }
            finally
            {
                mutexHandle?.Dispose();
            }
        }

        /// <summary>
        /// Попробовать установить блокировку для операции <paramref name="mutexName"/> с открытием транзакции
        /// При наличии блокировки будет выброшено исключение ValidateException с текстом <paramref name="lockFailMessage"/></summary>
        /// <param name="container">IWindsorContainer</param>
        /// <param name="mutexName">Имя операции</param>
        /// <param name="mutexDescription">Описание операции</param>
        /// <param name="lockFailMessage">Текст ошибки при наличии блокировки</param>
        /// <param name="action">Метод на выполнение</param>
        public static void InTransactionWithMutexTryLock(this IWindsorContainer container, string mutexName, string mutexDescription, string lockFailMessage, Action action)
        {
            container.InTransaction(() => container.InMutexTryLock(mutexName, mutexDescription, lockFailMessage, action));
        }

        /// <summary>
        /// Попробовать установить блокировку для операции <paramref name="mutexName"/>. 
        /// При наличии блокировки будет выброшено исключение ValidateException с текстом <paramref name="lockFailMessage"/></summary>
        /// <param name="container">IWindsorContainer</param>
        /// <param name="mutexName">Имя операции</param>
        /// <param name="mutexDescription">Описание операции</param>
        /// <param name="lockFailMessage">Текст ошибки при наличии блокировки</param>
        /// <param name="action">Метод на выполнение</param>
        public static void InMutexTryLock(this IWindsorContainer container, string mutexName, string mutexDescription, string lockFailMessage, Action action)
        {
            IDatabaseLockedMutexHandle mutexHandle = null;
            try
            {
                var databaseMutexManager = container.Resolve<IDatabaseMutexManager>();

                if (!databaseMutexManager.TryLock(mutexName, mutexDescription, out mutexHandle))
                {
                    throw new ValidationException(lockFailMessage);
                }

                action();
            }
            finally
            {
                mutexHandle?.Dispose();
            }
        }

        /// <summary>
        /// Выполняет операцию в транзакции и возвращает значение
        /// <para>
        /// Commit транзакции происходит при <see cref="IDataResult.Success"/> = true
        /// </para>
        /// </summary>
        /// <param name="container"><see cref="IWindsorContainer"/>Контейнер</param>
        /// <param name="func">Операция</param>
        /// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
        /// <returns><see cref="IDataResult"/></returns>
        /// <exception cref="ValidationException"><see cref="IDataResult.Success"/> = false</exception>
        /// <exception cref="DataAccessException"></exception>
        public static TResult InTransactionWithResult<TResult>(this IWindsorContainer container, Func<TResult> func) where TResult : IDataResult
        {
            TResult result;
            using (var transaction = container.Resolve<IDataTransaction>())
            {
                try
                {
                    result = func();
                    if (!result.Success)
                    {
                        throw new ValidationException(result.Message);
                    }

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            $"Произошла не известная ошибка при откате транзакции: \r\nMessage: {e.Message}; \r\nStackTrace:{e.StackTrace};",
                            exc);
                    }

                    throw;
                }
            }

            return result;
        }

        /// <summary>
        /// Выполняет операцию в переданном scope в транзакции и возвращает значение
        /// <para>
        /// Commit транзакции происходит при <see cref="IDataResult.Success"/> = true
        /// </para>
        /// </summary>
        /// <param name="container"><see cref="IWindsorContainer"/>Контейнер</param>
        /// <param name="scope">Scope</param>
        /// <param name="action">Операция</param>
        /// <exception cref="ValidationException"><see cref="IDataResult.Success"/> = false</exception>
        /// <exception cref="DataAccessException"></exception>
        public static void InTransactionInScope(this IWindsorContainer container, ILifetimeScope scope, Action action)
        {
            using (container.BeginScope())
            {
                container.InTransaction(action);
            }
        }

        /// <summary>
        /// Выполняет операцию в переданном scope в транзакции и возвращает значение
        /// <para>
        /// Commit транзакции происходит при <see cref="IDataResult.Success"/> = true
        /// </para>
        /// </summary>
        /// <param name="container"><see cref="IWindsorContainer"/>Контейнер</param>
        /// <param name="scope"></param>
        /// <param name="func">Операция</param>
        /// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
        /// <returns><see cref="IDataResult"/></returns>
        /// <exception cref="ValidationException"><see cref="IDataResult.Success"/> = false</exception>
        /// <exception cref="DataAccessException"></exception>
        public static TResult InTransactionWithResultInScope<TResult>(this IWindsorContainer container, IDisposable scope, Func<TResult> func) where TResult : IDataResult
        {
            using (container.BeginScope())
            {
                return container.InTransactionWithResult(func);
            }
        }

        /// <summary>
        /// Выполняет операцию в новом scope в транзакции
        /// <para>
        /// Commit транзакции происходит при <see cref="IDataResult.Success"/> = true
        /// </para>
        /// </summary>
        /// <param name="container"><see cref="IWindsorContainer"/>Контейнер</param>
        /// <param name="action">Операция</param>
        /// <exception cref="ValidationException"><see cref="IDataResult.Success"/> = false</exception>
        /// <exception cref="DataAccessException"></exception>
        public static void InTransactionInNewScope(this IWindsorContainer container, Action action)
        {
            using (container.BeginScope())
            {
                container.InTransaction(action);
            }
        }

        /// <summary>
        /// Выполняет операцию в новом scope в транзакции и возвращает значение
        /// <para>
        /// Commit транзакции происходит при <see cref="IDataResult.Success"/> = true
        /// </para>
        /// </summary>
        /// <param name="container"><see cref="IWindsorContainer"/>Контейнер</param>
        /// <param name="func">Операция</param>
        /// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
        /// <returns><see cref="IDataResult"/></returns>
        /// <exception cref="ValidationException"><see cref="IDataResult.Success"/> = false</exception>
        /// <exception cref="DataAccessException"></exception>
        public static TResult InTransactionWithResultInNewScope<TResult>(this IWindsorContainer container, Func<TResult> func) where TResult : IDataResult
        {
            using var newScope = container.BeginScope();
            return container.InTransactionWithResultInScope(newScope, func);
        }

        /// <summary>
        /// Выполняет операцию в новом scope в транзакции и возвращает значение
        /// <para>
        /// Commit транзакции происходит при <see cref="IDataResult.Success"/> = true
        /// </para>
        /// </summary>
        /// <param name="container"><see cref="IWindsorContainer"/>Контейнер</param>
        /// <param name="userIdentity">Идентификатор пользователя</param>
        /// <param name="func">Операция</param>
        /// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
        /// <returns><see cref="IDataResult"/></returns>
        /// <exception cref="ValidationException"><see cref="IDataResult.Success"/> = false</exception>
        /// <exception cref="DataAccessException"></exception>
        public static TResult InTransactionWithResultInNewScope<TResult>(this IWindsorContainer container, IUserIdentity userIdentity, Func<TResult> func) where TResult : IDataResult
        {
            using var newScope = container.BeginScope(userIdentity);
            return container.InTransactionWithResultInScope(newScope, func);
        }
    }
}