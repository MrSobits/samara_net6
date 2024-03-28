namespace Bars.Gkh.Entities.Administration.FormatDataExport
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.Security;

    /// <summary>
    /// Задача для экспорта данных по формату
    /// </summary>
    public class FormatDataExportTask : BaseSchedulableTask
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Выгружаемые группы секций
        /// </summary>
        public virtual IList<string> EntityGroupCodeList { get; set; }

        /// <summary>
        /// Признак удаления задачи
        /// </summary>
        public virtual bool IsDelete { get; set; }

        /// <summary>
        /// Параметры запуска задачи
        /// </summary>
        public virtual BaseParams BaseParams { get; set; }
    }
}