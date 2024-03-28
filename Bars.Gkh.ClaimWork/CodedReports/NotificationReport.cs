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

    /// <summary>
    /// Реализация BaseCodedReport, IClaimWorkCodedReport для уведомления
    /// </summary>
    public class NotificationReport : BaseCodedReport, IClaimWorkCodedReport
    {
        private string _outputFileName = "Уведомление";

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
            get { return "NotificationClw"; }
        }

        public override string Name
        {
            get { return "Уведомление_ПИР"; }
        }

        public override string Description => "Уведомление";

        public string CodeForm
        {
            get { return "NotificationClw"; }
        }

        public string OutputFileName
        {
            get { return _outputFileName; }
            set { _outputFileName = value; }
        }

        public ClaimWorkDocumentType DocumentType
        {
            get { return ClaimWorkDocumentType.Notification; }
        }

        public ClaimWorkReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            var provider = new NotificationDataProvider(ApplicationContext.Current.Container)
            {
                NotificationId = DocumentId
            };

            return new Collection<IDataSource>
            {
                new CodedDataSource("Уведомления", provider)
            };
        }

        public void Generate()
        {
            var generator = Container.Resolve<ICodedReportManager>();
            var notificationDomain = Container.ResolveDomain<NotificationClw>();

            try
            {
                if (DocumentId.IsEmpty())
                {
                    throw new Exception("Не найдено уведомление");
                }

                var notification = notificationDomain.Get(DocumentId.ToLong());

                if (notification == null)
                {
                    throw new Exception("Не найдена уведомление");
                }

                var claimWorkService = Container.ResolveAll<IClaimWorkService>()
                    .FirstOrDefault(x => x.TypeBase == notification.ClaimWork.ClaimWorkTypeBase);

                ReportInfo = claimWorkService != null ? claimWorkService.ReportInfoByClaimwork(notification.ClaimWork.Id) : new ClaimWorkReportInfo() { MunicipalityName = "" };

                OutputFileName = string.Format(
                    "Уведомление ({0}{1}).doc",
                    ReportInfo.Info,
                    notification.DocumentDate.HasValue
                        ? string.Format(" - {0}", notification.DocumentDate.Value.ToShortDateString())
                        : string.Empty)
                    .Replace("\"", "");

                ReportFileStream = generator.GenerateReport(this, null, ReportPrintFormat.docx);
            }
            finally
            {
                Container.Release(generator);
                Container.Release(notificationDomain);
            }
        }
    }
}