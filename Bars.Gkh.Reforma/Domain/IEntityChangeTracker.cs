namespace Bars.Gkh.Reforma.Domain
{
    using System;

    using Bars.Gkh.Reforma.Enums;

    /// <summary>
    /// Трекер изменения сущностей, задействованных в синхронизации с Реформой
    /// </summary>
    public interface IEntityChangeTracker
    {
        /// <summary>
        /// Оповещение об изменении сущности
        /// </summary>
        /// <param name="type">
        /// Тип сущности
        /// </param>
        /// <param name="entity">
        /// Сущность
        /// </param>
        void NotifyChanged(Type type, object entity);

        /// <summary>
        /// Оповещение об изменении сущности
        /// </summary>
        /// <typeparam name="TEntity">
        /// Тип сущности
        /// </typeparam>
        /// <param name="entity">
        /// Сущность
        /// </param>
        void NotifyChanged<TEntity>(TEntity entity);
    }
}