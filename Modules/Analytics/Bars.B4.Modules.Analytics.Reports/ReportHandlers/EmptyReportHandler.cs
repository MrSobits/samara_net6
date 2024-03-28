namespace Bars.B4.Modules.Analytics.Reports.ReportHandlers
{
    using Bars.B4.Modules.FileStorage;

    using Castle.Windsor;

    /// <summary>
    /// Пустой обработчик событий печати отчета
    /// </summary>
    public abstract class EmptyReportHandler : IReportHandler
    {
        /// <inheritdoc />
        public abstract string Code { get; }

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public virtual IDataResult BeforePrint(BaseParams baseParams)
        {
            return new BaseDataResult();
        }

        /// <inheritdoc />
        public virtual IDataResult AfterPrint(BaseParams baseParams, IDataResult<FileInfo> reportResult)
        {
            return new BaseDataResult();
        }
    }
}