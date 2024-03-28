namespace Bars.Gkh.FormatDataExport.NetworkWorker.Responses
{
    using Bars.Gkh.FormatDataExport.Enums;

    internal class DataSuccess
    {
        /// <summary>
        /// Идентификатор зачачи
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public FormadDataExportRemoteStatus Status { get; set; }
    }
}