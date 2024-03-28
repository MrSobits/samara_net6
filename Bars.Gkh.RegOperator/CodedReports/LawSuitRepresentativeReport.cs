﻿namespace Bars.Gkh.RegOperator.CodedReports
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
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Bars.Gkh.RegOperator.Properties;

    using Castle.Windsor;

    using Newtonsoft.Json;

    /// <summary>
    /// Реализация BaseCodedReport, IClaimWorkCodedReport для ЗВСП
    /// </summary>
    public class LawSuitRepresentativeReport : ExportFormatHelper, IClaimWorkCodedReport
    {
        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }
        
        protected override byte[] Template => Resources.LawSuitOwnerInfoReport;

        public string DocumentId { get; set; }

        public string Id => "LawSuitRepresentativeReport";

        public override string Name => "Исковое заявление (представитель несовершеннолетнего)";

        public override string Description
        {
            get { return this.Name; }
        }

        public string CodeForm => "LawSuitOwner";

        public string OutputFileName { get; set; } = "Исковое заявление (представитель несовершеннолетнего)";

        public ClaimWorkDocumentType DocumentType { get; } = ClaimWorkDocumentType.Lawsuit;

        public ClaimWorkReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new Collection<IDataSource>
            {
                new CodedDataSource("Список лицевых счетов", new LawSuitOwnerDataProvider(ApplicationContext.Current.Container)
                {
                    OwnerInfoIds = this.ReportInfo?.OnwerInfoIds ?? new long[0]
                })
            };
        }

        public void Generate()
        {
            var generator = this.Container.Resolve<ICodedReportManager>();
            var petitionDomain = this.Container.ResolveDomain<Petition>();
            var ownerInfoDomain = this.Container.ResolveDomain<LawsuitOwnerInfo>();
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

                var ownerInfo = ownerInfoDomain.Get(this.ReportInfo.OnwerInfoIds.FirstOrDefault());

                OperatorExportFormat format = GetExtension();
                this.OutputFileName =
                    $"Исковое заявление ({ownerInfo.Name} - {ownerInfo.PersonalAccount.PersonalAccountNum}"
                    + $"{(lawSuit.DocumentDate.HasValue ? $" - {lawSuit.DocumentDate.Value.ToShortDateString()}" : string.Empty)}).{format}"
                        .Replace("\"", "");

                this.ReportFileStream = generator.GenerateReport(this, null, (ReportPrintFormat)format);
            }
            finally
            {
                this.Container.Release(generator);
                this.Container.Release(petitionDomain);
                this.Container.Release(ownerInfoDomain);
            }
        }
    }
}