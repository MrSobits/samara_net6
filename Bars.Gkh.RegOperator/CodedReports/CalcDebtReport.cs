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
    /// Сопроводительное письмо в ЧЭС по переносу долга
    /// </summary>
    public class CalcDebtReport : BaseCodedReport
    {
        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        protected override byte[] Template => Resources.CalcDebtReport;

        public string CalcDebtId { get; set; }

        public string Id => "CalcDebtReport";

        public override string Name => "Сопроводительное письмо в ЧЭС по переносу долга";

        public override string Description => this.Name;

        public string CodeForm => "CalcDebtReport";

        public Stream ReportFileStream { get; set; }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new Collection<IDataSource>
            {
                new CodedDataSource("CalcDebtReport", new CalcDebtOperationDataProvider(ApplicationContext.Current.Container)
                {
                    CalcDebtId = this.CalcDebtId
                })
            };
        }
    }
}