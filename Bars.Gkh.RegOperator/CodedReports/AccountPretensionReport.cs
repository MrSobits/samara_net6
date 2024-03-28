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
    using Bars.Gkh.ClaimWork.Entities;
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
    public class AccountPretensionReport: BaseCodedReport, IClaimWorkCodedReport
    {
        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        protected override byte[] Template => Resources.LawSuitOwnerInfoReport;

        public string DocumentId { get; set; }

        public string Id => "AccountPretensionReport";

        public override string Name => "Претензия (лицевой счет)";

        public override string Description => this.Name;

        public string CodeForm => "PretensionAccount";

        public string OutputFileName { get; set; } = "Претензия";

        public long[] OnwerInfoIds { get; set; }

        public ClaimWorkDocumentType DocumentType => ClaimWorkDocumentType.Pretension;

        public ClaimWorkReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public long[] AccountInfoIds { get; set; }

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
            var pretensionDomain = this.Container.ResolveDomain<PretensionClw>();
            var accountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            try
            {
                if (this.DocumentId.IsEmpty())
                {
                    throw new Exception("Не найдена претензия");
                }

                var pretension = pretensionDomain.Get(this.DocumentId.ToLong());

                if (pretension == null)
                {
                    throw new Exception("Не найдена претензия");
                }

                var accountDetail = accountDetailDomain.Get(this.ReportInfo.OnwerInfoIds.FirstOrDefault());

                this.OutputFileName =
                    $"Претензия ({accountDetail.PersonalAccount.PersonalAccountNum} - {DateTime.Now.Date.ToShortDateString()}).doc"
                        .Replace("\"", "");

                this.ReportFileStream = generator.GenerateReport(this, null, ReportPrintFormat.docx);
            }
            finally
            {
                this.Container.Release(generator);
                this.Container.Release(pretensionDomain);
                this.Container.Release(accountDetailDomain);
            }
        }
    }
}