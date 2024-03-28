namespace Bars.Gkh.RegOperator.Report
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.Gkh.RegOperator.DataProviders.ChesImport;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;
    using Bars.Gkh.RegOperator.Properties;

    using Castle.Windsor;

    /// <summary>
    /// Отчет по результатам первичного импорта оплат и начислений
    /// </summary>
    public class ChesImportReport : BaseCodedReport
    {
        private readonly IWindsorContainer container;
        private readonly IChesImportService service;

        public ChesImportReport(IWindsorContainer container, IChesImportService service)
        {
            this.container = container;
            this.service = service;
        }

        /// <inheritdoc />
        protected override byte[] Template => Resources.ChesImportReport;

        /// <inheritdoc />
        public override string Name => "Отчет по результатам первичного импорта оплат и начислений";

        /// <inheritdoc />
        public override string Description => this.Name;

        /// <inheritdoc />
        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new[]
            {
                new CodedDataSource("ChargeSummaryData", new ChargeSummaryDataProvider(this.container, this.service)),
                new CodedDataSource("PaymentSummaryData", new PaymentSummaryDataProvider(this.container, this.service)),
                new CodedDataSource("SaldoChangeSummaryData", new SaldoChangeSummaryDataProvider(this.container, this.service)),
                new CodedDataSource("RecalcSummaryData", new RecalcSummaryDataProvider(this.container, this.service))
            };
        }
    }
}