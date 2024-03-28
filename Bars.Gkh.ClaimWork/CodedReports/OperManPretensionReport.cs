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
    using Entities;
    using Modules.ClaimWork.DomainService;
    using Modules.ClaimWork.Entities;
    using Modules.ClaimWork.Enums;
    using Newtonsoft.Json;
    using Properties;
    using Config;
    using Enums;

    /// <summary>
    /// Реализация BaseCodedReport, IClaimWorkCodedReport для претензии
    /// </summary>
    public class OperManPretensionReport : BaseCodedReport, IClaimWorkCodedReport
    {
        private string _outputFileName = "Претензия оперативное управление";

        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        protected override byte[] Template
        {
            get
            {
                return Resources.DocumentClwReport;
            }
        }

        public string DocumentId { get; set; }

        public string Id
        {
            get { return "OperManPretension"; }
        }

        public override string Name
        {
            get { return "Претензия оперативное управление"; }
        }

        public override string Description
        {
            get { return Name; }
        }

        public string CodeForm
        {
            get { return "Pretension"; }
        }

        public string OutputFileName
        {
            get { return _outputFileName; }
            set { _outputFileName = value; }
        }

        public ClaimWorkDocumentType DocumentType
        {
            get { return ClaimWorkDocumentType.Pretension; }
        }

        public ClaimWorkReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            var provider = new PretensionDataProvider(ApplicationContext.Current.Container)
            {
                PretensionId = DocumentId
            };

            return new Collection<IDataSource>
            {
                new CodedDataSource("Претензии оперативное управление", provider)
            };
        }

        public void Generate()
        {
            var generator = Container.Resolve<ICodedReportManager>();
            var pretensionDomain = Container.ResolveDomain<PretensionClw>();

            try
            {
                if (DocumentId.IsEmpty())
                {
                    throw new Exception("Не найдена претензия");
                }

                var pretension = pretensionDomain.Get(DocumentId.ToLong());

                if (pretension == null)
                {
                    throw new Exception("Не найдена претензия");
                }

                var claimWorkService = Container.ResolveAll<IClaimWorkService>()
                    .FirstOrDefault(x => x.TypeBase == pretension.ClaimWork.ClaimWorkTypeBase);

                ReportInfo = claimWorkService != null ? claimWorkService.ReportInfoByClaimwork(pretension.ClaimWork.Id) : new ClaimWorkReportInfo() { MunicipalityName = ""};

                OutputFileName = string.Format(
                        "Претензия оперативное управление ({0}{1}).doc",
                        ReportInfo.Info,
                        pretension.DocumentDate.HasValue
                            ? string.Format(" - {0}", pretension.DocumentDate.Value.ToShortDateString())
                            : string.Empty)
                    .Replace("\"", "");

                ReportFileStream = generator.GenerateReport(this, null, ReportPrintFormat.docx);
            }
            finally
            {
                Container.Release(generator);
                Container.Release(pretensionDomain);
            }
        }
    }
}