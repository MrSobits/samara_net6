namespace Bars.Gkh.Modules.ClaimWork.DomainService.ReportHandler
{
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Интерфейс обработчика печати документа
    /// </summary>
    public interface IClwReportExportHandler
    {
        /// <summary>
        /// Проверка, что указанный хендлер может обработать документ
        /// </summary>
        /// <param name="report">Отчет</param>
        /// <returns>Маркер, указывающий на возможность проверки</returns>
        bool CanHandle(IClaimWorkCodedReport report);

        /// <summary>
        /// Обработать экспорт документа, если возможно
        /// </summary>
        /// <param name="report">Отчет</param>
        /// <param name="file">Сформированный файл</param>
        void HandleExport(IClaimWorkCodedReport report, FileInfo file);
    }
}