namespace Bars.Gkh.Entities.Administration.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Результат выполнения действия
    /// </summary>
    public class ExecutionActionResult : BaseEntity
    {
        /// <summary>
        /// Задача экспорта
        /// </summary>
        public virtual ExecutionActionTask Task { get; set; }

        /// <summary>
        /// Дата запуска
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Дата завершения
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Статус выполнения
        /// </summary>
        public virtual ExecutionActionStatus Status { get; set; }

        /// <summary>
        /// Результат выполнения
        /// </summary>
        public virtual IDataResult Result { get; set; }
    }
}