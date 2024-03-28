namespace Bars.Gkh.Domain.TableLocker
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    ///     Сервис блокировки таблиц
    /// </summary>
    public interface ITableLocker : IDisposable
    {
        /// <summary>
        ///     Проверка существования блокировки для таблицы
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        bool CheckLocked(string tableName, string action);

        /// <summary>
        ///     Проверка существования блокировки для сущности
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        bool CheckLocked(Type type, string action);

        /// <summary>
        ///     Проверка существования блокировки для сущности
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        bool CheckLocked<T>(string action);

        /// <summary>
        ///     Блокировать таблицу по имени
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        ITableLocker Lock(string tableName, string action);

        /// <summary>
        ///     Блокировать таблицу по сущности
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ITableLocker Lock<T>(string action) where T : PersistentObject;

        /// <summary>
        ///     Блокировать таблицу по сущности
        /// </summary>
        /// <returns></returns>
        ITableLocker Lock(Type type, string action);

        /// <summary>
        ///     Бросать исключение при попытке заблокировать
        ///     уже заблокированную таблицу
        /// </summary>
        /// <param name="throwOn"></param>
        /// <returns></returns>
        ITableLocker ThrowOnAlreadyLocked(bool throwOn = true);

        /// <summary>
        ///     Разблокировать таблицу по имени
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        ITableLocker Unlock(string tableName, string action);

        /// <summary>
        ///     Разблокировать таблицу по сущности
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ITableLocker Unlock<T>(string action) where T : PersistentObject;

        /// <summary>
        ///     Разблокировать таблицу по сущности
        /// </summary>
        /// <returns></returns>
        ITableLocker Unlock(Type type, string action);
    }
}