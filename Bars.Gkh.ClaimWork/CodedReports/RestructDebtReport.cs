namespace Bars.Gkh.ClaimWork.CodedReports
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using B4.Modules.Analytics.Data;
    using B4.Modules.Analytics.Reports;
    using B4.Modules.Analytics.Reports.Enums;
    using B4.Modules.Analytics.Reports.Generators;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork;

    using Castle.Windsor;
    using DataProviders;
    using Modules.ClaimWork.DomainService;
    using Modules.ClaimWork.Entities;
    using Modules.ClaimWork.Enums;
    using Newtonsoft.Json;
    using Properties;

    /// <summary>
    /// Соглашение о погашении задолженности
    /// </summary>
    public class RestructDebtReport : BaseCodedReport, IClaimWorkCodedReport
    {
        private const string ReportName = "Соглашение о погашении задолженности";

        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        protected override byte[] Template => Resources.DocumentClwReport;

        public string DocumentId { get; set; }

        public virtual string Id => "RestructDebt";

        public override string Name => RestructDebtReport.ReportName;

        public override string Description => this.Name;

        public virtual string CodeForm => "RestructDebt";

        public virtual string OutputFileName { get; set; } = RestructDebtReport.ReportName;

        public virtual ClaimWorkDocumentType DocumentType => ClaimWorkDocumentType.RestructDebt;

        public ClaimWorkReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            var provider = new RestructDebtDataProvider(this.Container, this.Name)
            {
                DebtorClaimWorkId = this.DocumentId
            };

            return new Collection<IDataSource>
            {
                new CodedDataSource(this.Name, provider)
            };
        }

        public void Generate()
        {
            var generator = this.Container.Resolve<ICodedReportManager>();
            var restructDebtDomain = this.Container.ResolveDomain<RestructDebt>();

            using (this.Container.Using(generator, restructDebtDomain))
            {
                var document = restructDebtDomain.Get(this.DocumentId.ToLong());

                if (document == null)
                {
                    throw new Exception("Документ не найден");
                }

                var claimWorkService = this.Container.ResolveAll<IClaimWorkService>()
                    .FirstOrDefault(x => x.TypeBase == document.ClaimWork.ClaimWorkTypeBase);

                this.ReportInfo = claimWorkService != null
                    ? claimWorkService.ReportInfoByClaimwork(document.ClaimWork.Id)
                    : new ClaimWorkReportInfo { MunicipalityName = string.Empty };

                var date = document.DocumentDate.HasValue
                    ? $" - {document.DocumentDate.Value.ToShortDateString()}"
                    : string.Empty;

                this.OutputFileName = $"{this.Name} ({this.ReportInfo.Info}{date}).doc".Replace("\"", "");

                this.ReportFileStream = generator.GenerateReport(this, null, ReportPrintFormat.docx);
            }
        }
    }
}