namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Информация об изменения сущности
    /// </summary>
    public class EntityHistoryInfo : PersistentObject
    {
        /// <summary>
        /// Тип истории изменений
        /// </summary>
        public virtual EntityHistoryType GroupType { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public virtual DateTime EditDate { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public virtual ActionKind ActionKind { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public virtual string Username { get; set; }

        /// <summary>
        /// IP адрес
        /// </summary>
        public virtual string IpAddress { get; set; }

        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public virtual long EntityId { get; set; }

        /// <summary>
        /// Имя сущности <see cref="Type.FullName"/>
        /// </summary>
        public virtual string EntityName { get; set; }

        /// <summary>
        /// Идентификатор родительской сущности
        /// </summary>
        public virtual long ParentEntityId { get; set; }

        /// <summary>
        /// Имя сущности <see cref="Type.FullName"/>
        /// </summary>
        public virtual string ParentEntityName { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }
    }
}