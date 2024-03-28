namespace Bars.Gkh.ClaimWork.CodedReports
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using B4.Application;
    using B4.DataAccess;
    using B4.Modules.Analytics.Data;
    using B4.Modules.Analytics.Reports;
    using B4.Modules.Analytics.Reports.Enums;
    using B4.Modules.Analytics.Reports.Generators;
    using B4.Utils;

    using Bars.Gkh.Modules.ClaimWork;

    using Castle.Windsor;
    using DataProviders;
    using Modules.ClaimWork.DomainService;
    using Modules.ClaimWork.Entities;
    using Modules.ClaimWork.Enums;
    using Newtonsoft.Json;
    using Properties;

    /// <summary>
    /// Реализация BaseCodedReport, IClaimWorkCodedReport для искового заявления
    /// </summary>
    public class LawSuitLegalAccountReport : BaseCodedReport, IClaimWorkCodedReport
    {
        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        protected override byte[] Template => Resources.DocumentClwReport;

        public string DocumentId { get; set; }

        public string Id => "LawSuitLegal";

        public override string Name => "Исковое заявление (по абоненту)";

        public override string Description
        {
            get { return this.Name; }
        }

        public string CodeForm => "LawSuitLegalAccount";

        public string OutputFileName { get; set; } = "Исковое заявление";

        public ClaimWorkDocumentType DocumentType => ClaimWorkDocumentType.Lawsuit;

        public ClaimWorkReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            var provider = new LawSuitDataProvider(ApplicationContext.Current.Container)
            {
                LawSuitId = this.DocumentId
            };

            return new Collection<IDataSource>
            {
                new CodedDataSource("Исковые заявления", provider)
            };
        }

        public override Dictionary<string, string> GetExportSettings(ReportPrintFormat format)
        {
            if (format == ReportPrintFormat.docx)
            {
                return new Dictionary<string, string>
                {
                    { "RemoveEmptySpaceAtBottom", "false" }
                };
            }

            return null;
        }

        public void Generate()
        {
            var generator = this.Container.Resolve<ICodedReportManager>();
           //var petitionDomain = this.Container.ResolveDomain<Petition>();
            var lawsuitDomain = this.Container.ResolveDomain<Lawsuit>();
            try
            {
                if (this.DocumentId.IsEmpty())
                {
                    throw new Exception("Не найдено исковое заявление");
                }
                                

                var lawsuit = lawsuitDomain.Get(long.Parse(DocumentId));

               // this.ReportInfo = lawsuit != null ? new ClaimWorkReportInfo() { LawsuitId = lawsuit.Id} : new ClaimWorkReportInfo() { MunicipalityName = "" };

                this.OutputFileName = string.Format(
                    "Исковое заявление.doc");

                this.ReportFileStream = generator.GenerateReport(this, null, ReportPrintFormat.docx);
            }
            finally
            {
                this.Container.Release(generator);
                this.Container.Release(lawsuitDomain);
                // this.Container.Release(claimWorkServices);
            }
        }
    }
}