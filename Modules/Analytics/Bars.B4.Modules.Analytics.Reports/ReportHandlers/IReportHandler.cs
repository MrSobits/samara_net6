namespace Bars.B4.Modules.Analytics.Reports.ReportHandlers
{
    using Bars.B4.Modules.Analytics.Reports.Proxies;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Обработчик событий печати отчета
    /// </summary>
    public interface IReportHandler
    {
        /// <summary>
        /// Код отчета <see cref="Report.Code"/>
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Событие вызываемое перед печатью отчета
        /// </summary>
        IDataResult BeforePrint(BaseParams baseParams);

        /// <summary>
        /// Событие вызываемое после печати отчета
        /// </summary>
        IDataResult AfterPrint(BaseParams baseParams, IDataResult<FileInfo> reportResult);
    }
}