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
    public class LawSuitDeclaration512Report : BaseCodedReport, IClaimWorkCodedReport
    {
        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        protected override byte[] Template => Resources.DocumentClwReport;

        public string DocumentId { get; set; }

        public string Id => "LawSuitDeclaration512";

        public override string Name => "Заявление о вынесении судебного приказа 512 ФЗ";

        public override string Description => this.Name;

        public string CodeForm => "CourtClaim512";

        public string OutputFileName { get; set; } = "Заявление о вынесении судебного приказа 512 ФЗ";

        public ClaimWorkDocumentType DocumentType { get; } = ClaimWorkDocumentType.CourtOrderClaim;

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
                new CodedDataSource("Заявление о вынесении судебного приказа 512 ФЗ", provider)
            };
        }

        public void Generate()
        {
            var generator = this.Container.Resolve<ICodedReportManager>();
            var courtClaimDomain = this.Container.ResolveDomain<CourtOrderClaim>();
            var claimWorkServices = this.Container.ResolveAll<IClaimWorkService>();
            try
            {
                if (this.DocumentId.IsEmpty())
                {
                    throw new Exception("Не найдено исковое заявление");
                }

                var lawSuit = courtClaimDomain.Get(this.DocumentId.ToLong());

                if (lawSuit == null)
                {
                    throw new Exception("Не найдено исковое заявление");
                }

                var claimWorkService =
                    claimWorkServices.FirstOrDefault(x => x.TypeBase == lawSuit.ClaimWork.ClaimWorkTypeBase);

                this.ReportInfo = claimWorkService != null
                    ? claimWorkService.ReportInfoByClaimwork(lawSuit.ClaimWork.Id)
                    : new ClaimWorkReportInfo() {MunicipalityName = ""};

                this.OutputFileName = string.Format(
                    "Заявление о вынесении судебного приказа 512 ФЗ({0}{1}).doc",
                    this.ReportInfo.Info,
                    lawSuit.DocumentDate.HasValue
                        ? $" - {lawSuit.DocumentDate.Value.ToShortDateString()}"
                        : string.Empty)
                    .Replace("\"", "");

                this.ReportFileStream = generator.GenerateReport(this, null, ReportPrintFormat.docx);
            }
            finally
            {
                this.Container.Release(generator);
                this.Container.Release(courtClaimDomain);
                this.Container.Release(claimWorkServices);
            }
        }
    }
}