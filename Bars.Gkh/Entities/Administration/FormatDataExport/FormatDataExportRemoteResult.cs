namespace Bars.Gkh.Entities.Administration.FormatDataExport
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.FormatDataExport.Enums;

    /// <summary>
    /// Результат экспорта данных в удаленной системе
    /// </summary>
    public class FormatDataExportRemoteResult : BaseEntity
    {
        /// <summary>
        /// Идентификатор файла на удаленном сервере
        /// </summary>
        public virtual long? FileId { get; set; }

        /// <summary>
        /// Идентификатор задачи на удаленном сервере
        /// </summary>
        public virtual long? TaskId { get; set; }

        /// <summary>
        /// Идентификатор файла лога на удаленном сервере
        /// </summary>
        public virtual long? LogId { get; set; }

        /// <summary>
        /// Статус задачи на удаленном сервере
        /// </summary>
        public virtual FormadDataExportRemoteStatus Status { get; set; }

        /// <summary>
        /// Результат экспорта данных
        /// </summary>
        public virtual FormatDataExportResult TaskResult { get; set; }

        /// <summary>
        /// Резальтат загрузки файла
        /// </summary>
        public virtual IDataResult UploadResult { get; set; }
    }
}