namespace Bars.Gkh.Overhaul.Tat.Entities
{
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
    }
}
