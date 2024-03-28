namespace Bars.Gkh.ClaimWork.CodedReports
{
    using B4.Application;
    using B4.DataAccess;
    using B4.Modules.Analytics.Data;
    using B4.Modules.Analytics.Reports;
    using B4.Modules.Analytics.Reports.Enums;
    using B4.Modules.Analytics.Reports.Generators;
    using B4.Utils;

    using Bars.B4;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    using Castle.Windsor;

    using DataProviders;

    using Gkh.ClaimWork.Properties;

    using Modules.ClaimWork.DomainService;

    using Newtonsoft.Json;

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Bars.Gkh.Modules.ClaimWork;

    /// <summary>
    /// 
    /// </summary>
    public class ApplicationToRenounceClaimsReport : BaseCodedReport, IClaimWorkCodedReport
    {
        private string outputFileName = "Заявление об отказе от исковых требований";

        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override byte[] Template
        {
            get { return Resources.DocumentClwReport; }
        }

        /// <summary>
        /// </summary>
        public override string Name
        {
            get { return "Заявление об отказе от исковых требований"; }
        }

        /// <summary>
        /// </summary>
        public override string Description
        {
            get { return this.Name; }
        }

        /// <summary>
        /// </summary>
        /// <returns/>
        public override IEnumerable<IDataSource> GetDataSources()
        {
            var provider = new LawSuitDataProvider(ApplicationContext.Current.Container)
            {
                LawSuitId = this.DocumentId
            };

            return new Collection<IDataSource>
            {
                new CodedDataSource("Заявление об отказе от исковых требований", provider)
            };
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public string Id
        {
            get { return "ApplicationToRenounceClaims"; }
        }

        /// <summary>
        /// Идентификатор документа для печати
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public string CodeForm
        {
            get { return "RefusalLawsuit"; }
        }

        /// <summary>
        /// Название выходного файла
        /// </summary>
        public string OutputFileName
        {
            get { return this.outputFileName; }
            set { this.outputFileName = value; }
        }

        /// <summary>
        /// Тип документа ПиР
        /// </summary>
        public ClaimWorkDocumentType DocumentType
        {
            get { return ClaimWorkDocumentType.RefusalLawsuit; }
        }

        /// <summary>
        /// Информация
        /// </summary>
        public ClaimWorkReportInfo ReportInfo { get; set; }

        /// <summary>
        /// Stream сгенерированной печатной формы
        /// </summary>
        public System.IO.Stream ReportFileStream { get; set; }

        /// <summary>
        /// Генерация документа для выгрузки
        /// </summary>
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

                this.ReportInfo = claimWorkService != null
                    ? claimWorkService.ReportInfoByClaimwork(lawSuit.ClaimWork.Id)
                    : new ClaimWorkReportInfo() {MunicipalityName = ""};

                this.OutputFileName = string.Format(
                    "Заявление об отказе от исковых требований ({0}{1}).doc", this.ReportInfo.Info,
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