namespace Bars.Gkh.FormatDataExport.NetworkWorker.Responses
{
    using Bars.Gkh.FormatDataExport.Enums;

    internal class StatusSuccess
    {
        /// <summary>
        /// Идентификатор загрузки
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Оригинальное имя файла
        /// </summary>
        public FormadDataExportRemoteStatus Status { get; set; }

        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public long? FileId { get; set; }

        /// <summary>
        /// Идентификатор лога
        /// </summary>
        public long? LogId { get; set; }
    }
}