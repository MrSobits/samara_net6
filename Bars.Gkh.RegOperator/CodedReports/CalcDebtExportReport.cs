namespace Bars.Gkh.RegOperator.CodedReports
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;

    using Bars.B4.Application;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.Properties;

    using Castle.Windsor;

    using Newtonsoft.Json;

    /// <summary>
    /// Отчёт по переносу долга в ЧЭС
    /// </summary>
    public class CalcDebtExportReport : BaseCodedReport
    {
        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        protected override byte[] Template => Resources.CalcDebtExportReport;

        public string CalcDebtId { get; set; }

        public string Id => "CalcDebtExportReport";

        public override string Name => "Отчёт по переносу долга в ЧЭС";

        public override string Description => this.Name;

        public string CodeForm => "CalcDebtExportReport";

        public Stream ReportFileStream { get; set; }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new Collection<IDataSource>
            {
                new CodedDataSource("CalcDebtExportReport", new CalcDebtOperationDataProvider(ApplicationContext.Current.Container)
                {
                    CalcDebtId = this.CalcDebtId
                })
            };
        }
    }
}