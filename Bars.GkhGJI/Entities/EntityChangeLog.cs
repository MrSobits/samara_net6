namespace Bars.GkhGji.Entities
{
    using B4.DataAccess;
    using Enums;

    public class EntityChangeLogRecord : BaseEntity
    {
        /// <summary>
        /// Ид логируемой сущности
        /// </summary>
        public virtual long EntityId { get; set; }

        /// <summary>
        /// Тип логируемой сущности
        /// </summary>
        public virtual TypeEntityLogging TypeEntityLogging { get; set; }

        /// <summary>
        /// Тип операции
        /// </summary>
        public virtual AppealOperationType OperationType { get; set; }

        /// <summary>
        /// Старое значение
        /// </summary>
        public virtual string OldValue { get; set; }

        /// <summary>
        /// Новое значение
        /// </summary>
        public virtual string NewValue { get; set; }

        /// <summary>
        /// Тип свойства
        /// </summary>
        public virtual string PropertyType { get; set; }

        /// <summary>
        /// Наименование свойства
        /// </summary>
        public virtual string PropertyName { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public virtual string OperatorLogin { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public virtual string OperatorName { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public virtual long OperatorId { get; set; }

        /// <summary>
        /// Значение логируемой сущности
        /// </summary>
        public virtual string DocumentValue { get; set; }

        /// <summary>
        /// Родительская сущность(при наличии)
        /// </summary>
        public virtual long? ParrentEntity { get; set; }
    }
}