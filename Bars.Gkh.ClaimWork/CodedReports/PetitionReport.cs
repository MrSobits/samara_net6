namespace Bars.Gkh.ClaimWork.CodedReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections.ObjectModel;
    using System.IO;
    using B4.Application;
    using B4.DataAccess;
    using B4.Modules.Analytics.Data;
    using B4.Modules.Analytics.Reports;
    using B4.Modules.Analytics.Reports.Enums;
    using B4.Modules.Analytics.Reports.Generators;
    using B4.Utils;

    using DataProviders;
    using Properties;
    using Modules.ClaimWork;
    using Modules.ClaimWork.DomainService;
    using Modules.ClaimWork.Entities;
    using Modules.ClaimWork.Enums;

    using Castle.Windsor;
    using Newtonsoft.Json;

    /// <summary>
    /// Реализация BaseCodedReport, IClaimWorkCodedReport для Ходатайство
    /// </summary>
    public class PetitionReport : BaseCodedReport, IClaimWorkCodedReport
    {
        private string outputFileName = "Ходатайство";

        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        protected override byte[] Template => Resources.DocumentClwReport;

        public string DocumentId { get; set; }

        public string Id => "PetitionClw";

        public override string Name => "Ходатайство";

        public override string Description => "Ходатайство";

        public string CodeForm => "PetitionClw";

        public string OutputFileName { get; set; } = "Ходатайство";

        public ClaimWorkDocumentType DocumentType => ClaimWorkDocumentType.Notification;

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
                new CodedDataSource("Ходатайство", provider)
            };
        }

        public void Generate()
        {
            var generator = this.Container.Resolve<ICodedReportManager>();
            var courtClaimDomain = this.Container.ResolveDomain<CourtOrderClaim>();
            var petitionDomain = this.Container.ResolveDomain<Petition>();
            var claimWorkServices = this.Container.ResolveAll<IClaimWorkService>();
            try
            {
                if (this.DocumentId.IsEmpty())
                {
                    throw new Exception("Не найдено исковое заявление");
                }

                Petition petition;
                CourtOrderClaim courtClaim;

                courtClaim = courtClaimDomain.Get(this.DocumentId.ToLong());
                petition = petitionDomain.Get(this.DocumentId.ToLong());

                var сlaimWork = petition?.ClaimWork ?? courtClaim?.ClaimWork;


                if (сlaimWork == null)
                {
                    throw new Exception("Не найдено исковое заявление");
                }

                var claimWorkService =
                    claimWorkServices.FirstOrDefault(x => x.TypeBase == сlaimWork.ClaimWorkTypeBase);

                this.ReportInfo = claimWorkService != null
                    ? claimWorkService.ReportInfoByClaimwork(сlaimWork.Id)
                    : new ClaimWorkReportInfo() { MunicipalityName = "" };

                this.OutputFileName = $"Ходатайство({this.ReportInfo.Info}{(сlaimWork.StartingDate.HasValue ? $" - {сlaimWork.StartingDate.Value.ToShortDateString()}" : string.Empty)}).doc"
                    .Replace("\"", "");

                this.ReportFileStream = generator.GenerateReport(this, null, ReportPrintFormat.docx);
            }
            finally
            {
                this.Container.Release(generator);
                this.Container.Release(courtClaimDomain);
                this.Container.Release(petitionDomain);
                this.Container.Release(claimWorkServices);
            }
        }
    }
}