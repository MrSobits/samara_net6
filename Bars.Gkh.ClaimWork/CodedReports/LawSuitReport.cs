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
    public class LawSuitReport : BaseCodedReport, IClaimWorkCodedReport
    {
        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        protected override byte[] Template => Resources.DocumentClwReport;

        public string DocumentId { get; set; }

        public string Id => "LawSuit";

        public override string Name => "Исковое заявление";

        public override string Description => this.Name;

        public string CodeForm => "LawSuit";

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

        public void Generate()
        {
            var generator = this.Container.Resolve<ICodedReportManager>();
            var petitionDomain = this.Container.ResolveDomain<Petition>();
            var claimWorkServices = this.Container.ResolveAll<IClaimWorkService>();
            try
            {
                if (this.DocumentId.IsEmpty())
                {
                    throw new Exception("Не найдено исковое заявление");
                }

                var lawSuit = petitionDomain.Get(this.DocumentId.ToLong());

                if (lawSuit == null)
                {
                    throw new Exception("Не найдено исковое заявление");
                }

                var claimWorkService =
                    claimWorkServices.FirstOrDefault(x => x.TypeBase == lawSuit.ClaimWork.ClaimWorkTypeBase);

                this.ReportInfo = claimWorkService != null ? claimWorkService.ReportInfoByClaimwork(lawSuit.ClaimWork.Id) : new ClaimWorkReportInfo() { MunicipalityName = "" };

                this.OutputFileName = string.Format(
                    "Исковое заявление ({0}{1}).doc",
                    this.ReportInfo.Info,
                    lawSuit.DocumentDate.HasValue
                        ? string.Format(" - {0}", lawSuit.DocumentDate.Value.ToShortDateString())
                        : string.Empty)
                    .Replace("\"", "");

                this.ReportFileStream = generator.GenerateReport(this, null, ReportPrintFormat.docx);
            }
            finally
            {
                this.Container.Release(generator);
                this.Container.Release(petitionDomain);
                this.Container.Release(claimWorkServices);
            }
        }
    }
}