namespace Bars.Gkh.Entities
{
    using System;

    /// <summary>
    ///     Блокировка изменения данных
    /// </summary>
    public class TableLock
    {
        /// <summary>
        ///     Блокируемое действие
        /// </summary>
        public virtual string Action { get; set; }

        /// <summary>
        ///     Дата установки блокировки
        /// </summary>
        public virtual DateTime LockStart { get; set; }

        /// <summary>
        ///     Имя таблицы
        /// </summary>
        public virtual string TableName { get; set; }
    }
}