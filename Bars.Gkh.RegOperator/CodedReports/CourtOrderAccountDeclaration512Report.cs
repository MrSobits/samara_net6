namespace Bars.Gkh.RegOperator.CodedReports
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Properties;

    using Castle.Windsor;

    using Newtonsoft.Json;

    /// <summary>
    /// Реализация BaseCodedReport, IClaimWorkCodedReport для искового заявления
    /// </summary>
    public class CourtOrderAccountDeclaration512Report : ExportFormatHelper, IClaimWorkCodedReport
    {
        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        protected override byte[] Template => Resources.LawSuitOwnerDeclarationReport;

        public string DocumentId { get; set; }

        public string Id => "CourtOrderAccountDeclaration512Report";

        public override string Name => "Заявление о выдаче судебного приказа 512 ФЗ (лицевой счет)";

        public override string Description => this.Name;

        public string CodeForm => "AccountClaim512";

        public string OutputFileName { get; set; } = "Заявление о выдаче судебного приказа 512 ФЗ";

        public ClaimWorkDocumentType DocumentType { get; } = ClaimWorkDocumentType.CourtOrderClaim;

        public ClaimWorkReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new Collection<IDataSource>
            {
                new CodedDataSource("Список лицевых счетов", new LawSuitAccountDataProvider(ApplicationContext.Current.Container)
                {
                    AccountInfoIds = this.ReportInfo?.OnwerInfoIds ?? new long[0]
                })
            };
        }

        public void Generate()
        {
            var generator = this.Container.Resolve<ICodedReportManager>();
            var courtClaimDomain = this.Container.ResolveDomain<CourtOrderClaim>();
            var accountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var claimWorkServices = this.Container.ResolveAll<IClaimWorkService>();
            try
            {
                if (this.DocumentId.IsEmpty())
                {
                    throw new Exception("Не найдено заявление о выдаче судебного приказа 512 ФЗ");
                }

                var lawSuit = courtClaimDomain.Get(this.DocumentId.ToLong());

                if (lawSuit == null)
                {
                    throw new Exception("Не найдено заявление о выдаче судебного приказа 512 ФЗ");
                }

                var accountDetail = accountDetailDomain.Get(this.ReportInfo.OnwerInfoIds.FirstOrDefault());

                if (accountDetail == null)
                {
                    var claimWorkService =
                        claimWorkServices.FirstOrDefault(x => x.TypeBase == lawSuit.ClaimWork.ClaimWorkTypeBase);

                    this.ReportInfo = claimWorkService != null ? claimWorkService.ReportInfoByClaimwork(lawSuit.ClaimWork.Id) : new ClaimWorkReportInfo() { MunicipalityName = "" };
                }

                OperatorExportFormat format = GetExtension();
                this.OutputFileName = "ReportTest.doc";
                this.OutputFileName =
                    $"Заявление о выдаче судебного приказа 512 ФЗ ({ReportInfo.Info} - {DateTime.Now.Date.ToShortDateString()}).{format}"
                        .Replace("\"", "");

                this.ReportFileStream = generator.GenerateReport(this, null, (ReportPrintFormat)format);
            }
            finally
            {
                this.Container.Release(generator);
                this.Container.Release(courtClaimDomain);
                this.Container.Release(accountDetailDomain);
            }
        }
    }
}