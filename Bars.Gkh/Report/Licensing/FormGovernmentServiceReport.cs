namespace Bars.Gkh.Report.Licensing
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.Gkh.DataProviders;
    using Bars.Gkh.Enums.Licensing;
    using Bars.Gkh.Properties;

    using Castle.Windsor;

    /// <summary>
    /// Отчёт Форма 1-ГУ
    /// </summary>
    public class FormGovernmentServiceReport : BaseCodedReport
    {
        private readonly IWindsorContainer container;

        /// <inheritdoc />
        public override string Name => "Форма 1-ГУ";

        /// <inheritdoc />
        public override string Description { get; }

        /// <inheritdoc />
        protected override byte[] Template => Resources.FormGovernmentServiceReport;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container">Контейнер</param>
        public FormGovernmentServiceReport(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc />
        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new List<IDataSource>
            {
                new CodedDataSource("Шапка", new FormGovernmentServiceDataProvider(this.container)),
                new CodedDataSource("Раздел1", new GovernmenServiceDetailDataProvider(this.container, ServiceDetailSectionType.PublicServices)),
                new CodedDataSource("Раздел2", new GovernmenServiceDetailDataProvider(this.container, ServiceDetailSectionType.ServiceDelivery)),
                new CodedDataSource("Раздел3", new GovernmenServiceDetailDataProvider(this.container, ServiceDetailSectionType.ServiceTime)),
                new CodedDataSource("Раздел4", new GovernmenServiceDetailDataProvider(this.container, ServiceDetailSectionType.AppealAndDecisions))
            };
        }
    }
}