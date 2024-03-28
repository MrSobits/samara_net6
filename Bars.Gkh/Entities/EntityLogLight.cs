namespace Bars.Gkh.Entities
{
    using System;
    using B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Легковесная сущность для хранения изменения сущности
    /// </summary>
    public class EntityLogLight : BaseImportableEntity
    {
        /// <summary>
        /// Идентификатор сущности 
        /// </summary>
        public virtual long EntityId { get; set; }

        /// <summary>
        /// Класс сущности
        /// </summary>
        public virtual string ClassName { get; set; }

        /// <summary>
        /// Описание сущности
        /// </summary>
        public virtual string ClassDescription { get; set; }

        /// <summary>
        /// Имя свойства
        /// </summary>
        public virtual string PropertyName { get; set; }

        /// <summary>
        /// Описание измененного атрибута
        /// </summary>
        public virtual string PropertyDescription { get; set; }

        /// <summary>
        /// Значение свойства
        /// </summary>
        public virtual string PropertyValue { get; set; }

        /// <summary>
        /// Дата поступления сведений об изменении значения
        /// </summary>
        public virtual DateTime? DateApplied { get; set; }

        /// <summary>
        /// Дата начала действия значения
        /// </summary>
        public virtual DateTime DateActualChange { get; set; }

        /// <summary>
        /// Дата окончания действия нового значения
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Документ - основание
        /// </summary>
        public virtual FileInfo Document { get; set; }

        /// <summary>
        /// Наименование параметра
        /// </summary>
        public virtual string ParameterName { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual string User { get; set; }

        /// <summary>
        /// Использовалось при перерасчете
        /// </summary>
        public virtual bool UsedInRecalculation { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Reason { get; set; }
    }
}