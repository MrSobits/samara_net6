namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Логируемое поле сущности
    /// </summary>
    public class EntityHistoryField : PersistentObject
    {
        /// <summary>
        /// Старое значение
        /// </summary>
        public virtual string OldValue { get; set; }

        /// <summary>
        /// Новое значение
        /// </summary>
        public virtual string NewValue { get; set; }

        /// <summary>
        /// Наименование поля
        /// </summary>
        public virtual string FieldName { get; set; }

        /// <summary>
        /// Информация об изменении
        /// </summary>
        public virtual EntityHistoryInfo EntityHistoryInfo { get; set; }
    }
}