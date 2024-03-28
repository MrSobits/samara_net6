namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System.Collections.Generic;
    using B4.DataAccess;

    /// <summary>
    /// Множественные параметры
    /// </summary>
    public class MultiPriorityParam : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// Балл
        /// </summary>
        public virtual decimal Point { get; set; }

        /// <summary>
        /// Записи справочника
        /// </summary>
        public virtual List<StoredMultiValue> StoredValues { get; set; }
    }

    public class StoredMultiValue
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Code { get; set; }
    }
}
