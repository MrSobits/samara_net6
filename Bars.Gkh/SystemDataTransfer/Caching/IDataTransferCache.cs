namespace Bars.Gkh.SystemDataTransfer.Caching
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.SystemDataTransfer.Meta;

    /// <summary>
    /// Кэш импорта системы
    /// </summary>
    public interface IDataTransferCache: IDisposable
    {
        /// <summary>
        /// Зарегистрировать сущность в кэше
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="meta">Мета-поисание</param>
        void RegisterEntityCacheMap<TEntity>(ITransferEntityMeta meta)
            where TEntity : class, IEntity;

        /// <summary>
        /// Зарегистрировать для сущности <typeparamref name="TEntity"/> зависимость
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="depencyMeta">Мета-описание типа</param>
        void RegisterDependecy<TEntity>(string propertyName, ITransferEntityMeta depencyMeta)
            where TEntity : class, IEntity;

        /// <summary>
        /// Добавить наследника
        /// </summary>
        /// <typeparam name="TEnity">Тип сущности</typeparam>
        /// <param name="inheritMeta">Мета-описание типа наследника</param>
        void AddInheritance<TEnity>(ITransferEntityMeta inheritMeta);

        /// <summary>
        /// Метод пытается найти внутренний идентификатор по внешнему
        /// </summary>
        /// <param name="type">Тип сущности</param>
        /// <param name="importId">Внешний идентификатор</param>
        /// <param name="entityId">Внутренний идентификатор</param>
        /// <returns></returns>
        bool TryGetDependencyId(Type type, object importId, out object entityId);

        /// <summary>
        /// Метод пытается найти внутренний идентификатор по внешнему
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="importId">Внешний идентификатор</param>
        /// <param name="entityId">Внутренний идентификатор</param>
        /// <returns></returns>
        bool TryGetDependencyId<TEntity>(object importId, out object entityId) 
            where TEntity : class, IEntity;

        /// <summary>
        /// Метод пытается найти сущность в кэше (сначала по внешнему id, потом по карте сопоставления)
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="objectMap">Входящие данные</param>
        /// <param name="entity">Полученная сущность</param>
        /// <returns>Успешность операции</returns>
        bool TryGetEnitity<TEntity>(IDictionary<string, object> objectMap, out TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        /// Метод пытается найти внутренний идентификатор в кэше (сначала по внешнему id, потом по карте сопоставления)
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="objectMap">Входящие данные</param>
        /// <param name="entityId">Dнутренний идентификатор</param>
        /// <returns>Успешность операции</returns>
        bool TryGetId<TEntity>(IDictionary<string, object> objectMap, out object entityId);

        /// <summary>
        /// Добавить добавленную сущность в кэш
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="objectMap">Входящие данные</param>
        void AddEntity(IEntity entity, IDictionary<string, object> objectMap);

        /// <summary>
        /// Прогреть кэш
        /// </summary>
        void WarmCache();
    }
}