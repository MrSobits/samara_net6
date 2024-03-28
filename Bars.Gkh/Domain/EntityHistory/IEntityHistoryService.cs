namespace Bars.Gkh.Domain.EntityHistory
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;

    /// <summary>
    /// Сервис логирования изменений сущности <see cref="T"/>
    /// </summary>
    /// <typeparam name="T">Тип логируемой сущности</typeparam>
    public interface IEntityHistoryService<in T>
        where T : PersistentObject
    {
        /// <summary>
        /// Зафиксировать состояние сущности
        /// </summary>
        /// <param name="entityId">Идентификатор изменяемой сущности</param>
        void StoreEntity(long entityId);

        /// <summary>
        /// Залогировать создание сущности
        /// </summary>
        /// <param name="entity">Логируемая сущность</param>
        /// <param name="parentEntity">Идентификатор родительской сущности. По умолчанию: идентификатор сущности <see cref="IHaveId.Id"/></param>
        void LogCreate(T entity, IHaveId parentEntity = null);

        /// <summary>
        /// Залогировать изменение сущности
        /// </summary>
        /// <param name="entity">Логируемая сущность</param>
        /// <param name="parentEntity">Идентификатор родительской сущности. По умолчанию: идентификатор сущности <see cref="IHaveId.Id"/></param>
        void LogUpdate(T entity, IHaveId parentEntity = null);

        /// <summary>
        /// Залогировать удаление сущности
        /// </summary>
        /// <param name="parentEntity">Идентификатор родительской сущности. По умолчанию: идентификатор сущности <see cref="IHaveId.Id"/></param>
        void LogDelete(IHaveId parentEntity = null);
    }
}