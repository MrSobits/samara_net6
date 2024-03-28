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
    using Bars.Gkh.Modules.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Properties;

    using Castle.Windsor;

    using Newtonsoft.Json;

    using Entities.Owner;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Authentification;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Заявление о возбуждении исполнительного производства
    /// </summary>
    public class ExecutiveClaimReport : ExportFormatHelper, IClaimWorkCodedReport
    {
        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        protected override byte[] Template => Resources.LawSuitOwnerInfoReport;

        public string DocumentId { get; set; }

        public string ClaimworkId { get; set; }

        public string OwnerId { get; set; }

        public bool Solidary { get; set; }
        public string FIO { get; set; }

        public string Id => "ExecutiveClaim";

        public override string Name => "Заявление о возбуждении исполнительного производства";

        public override string Description
        {
            get { return this.Name; }
        }

        public string CodeForm => "ExecutiveClaim";

        public string OutputFileName { get; set; } = "Заявление о возбуждении исполнительного производства";

        public ClaimWorkDocumentType DocumentType => ClaimWorkDocumentType.Lawsuit;

        public ClaimWorkReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public long rloi_id { get; set; }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            string fio = string.Empty;
            string pos = string.Empty;
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator.Inspector != null)
            {
                fio = thisOperator.Inspector.Fio;
                this.FIO = thisOperator.Inspector.Fio;
                pos = thisOperator.Inspector.Position;
            }
                return new Collection<IDataSource>
            {       
            
                new CodedDataSource("ClaimworkInfo", new ClaimworkAllDataProvider(ApplicationContext.Current.Container)
                {
                    ClaimworkId = ClaimworkId,
                    LawsuitId = DocumentId,
                    OwnerId = OwnerId,
                    Solidary = Solidary,
                    FIO = FIO,
                    Pos = pos
                })
            };
        }

        public override Dictionary<string, string> GetExportSettings(ReportPrintFormat format)
        {
            if (format == ReportPrintFormat.docx)
            {
                return new Dictionary<string, string>
                {
                    { "RemoveEmptySpaceAtBottom" , "false" }
                };
            }

            return null;
        }

        public void Generate()
        {
            var generator = this.Container.Resolve<ICodedReportManager>();
            var petitionDomain = this.Container.ResolveDomain<Petition>();
            var accountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var claimWorkServices = this.Container.ResolveAll<IClaimWorkService>();
            var lawsuitOwnerDomain = this.Container.ResolveDomain<LawsuitOwnerInfo>();
            var documentDomain = this.Container.ResolveDomain<DocumentClw>();
            try
            {
                if (this.DocumentId.IsEmpty())
                {
                    throw new Exception("Не найдено исковое заявление");
                }         


                    long intOwnerId;
                Int64.TryParse(OwnerId, out intOwnerId);
                var lawsuitOwner = lawsuitOwnerDomain.Get(intOwnerId);

                OperatorExportFormat format = GetExtension();
                this.OutputFileName =
                    $"{OutputFileName} ({lawsuitOwner?.Name} - {DateTime.Now.Date.ToShortDateString()}).{format}"
                        .Replace("\"", "");

                this.ReportFileStream = generator.GenerateReport(this, null, (ReportPrintFormat)format);
            }
            finally
            {
                this.Container.Release(generator);
                this.Container.Release(petitionDomain);
                this.Container.Release(accountDetailDomain);
                this.Container.Release(claimWorkServices);
            }
        }
    }
}