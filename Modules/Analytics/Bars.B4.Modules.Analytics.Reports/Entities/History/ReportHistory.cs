namespace Bars.B4.Modules.Analytics.Reports.Entities.History
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;

    /// <summary>
    /// Журнал созданных отчетов
    /// </summary>
    public class ReportHistory : BaseEntity
    {
        /// <summary>
        /// Тип отчета
        /// </summary>
        public virtual ReportType ReportType { get; set; }

        /// <summary>
        /// Id отчета (старого или нового)
        /// </summary>
        public virtual long? ReportId { get; set; }

        /// <summary>
        /// Дата печати
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Категория отчета
        /// </summary>
        public virtual PrintFormCategory Category { get; set; }

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Файл отчета
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Пользователь, запустивший отчет
        /// </summary>
        public virtual User User { get; set; }     

        /// <summary>
        /// Словарь значений параметров
        /// </summary>
        public virtual Dictionary<string, ReportHistoryParam> ParameterValues { get; set; }

        public ReportHistory()
        {
            this.ParameterValues = new Dictionary<string, ReportHistoryParam>();
        }
    }
}