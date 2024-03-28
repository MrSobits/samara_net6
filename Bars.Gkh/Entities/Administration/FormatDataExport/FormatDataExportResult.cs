namespace Bars.Gkh.Entities.Administration.FormatDataExport
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Результат экспорта данных
    /// </summary>
    public class FormatDataExportResult : BaseEntity
    {
        /// <summary>
        /// Задача экспорта
        /// </summary>
        public virtual FormatDataExportTask Task { get; set; }

        /// <summary>
        /// Статус экспорта
        /// </summary>
        public virtual FormatDataExportStatus Status { get; set; }

        /// <summary>
        /// Дата запуска
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Дата завершения
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Прогресс
        /// </summary>
        public virtual float Progress { get; set; }

        /// <summary>
        /// Лог операции
        /// </summary>
        public virtual LogOperation LogOperation { get; set; }

        /// <summary>
        /// Коды экспортированных сущностей
        /// </summary>
        public virtual IList<string> EntityCodeList { get; set; }
    }
}