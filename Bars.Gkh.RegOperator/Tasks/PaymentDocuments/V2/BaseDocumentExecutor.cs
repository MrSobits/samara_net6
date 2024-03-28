namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2
{
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enums.PaymentDocumentOptions;
    using Bars.Gkh.RegOperator.Report.ReportManager;
    using Castle.Windsor;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.PersonalAccountGroup;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Базывый класс для обработчиков задачи выгрузки документов на оплату
    /// </summary>
    internal abstract class BaseDocumentExecutor : ITaskExecutor
    {
        protected ILogger appLogger;
        private readonly IWindsorContainer container;
        private readonly IDomainService<PaymentDocumentLog> paymentDocumentLogDomain;
        private readonly IDomainService<PaymentDocumentAccountLog> paymentDocumentAccountLogDomain;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        protected BaseDocumentExecutor(IWindsorContainer container)
        {
            this.container = container;
            this.appLogger = container.Resolve<ILogger>();

            this.paymentDocumentLogDomain = this.container.ResolveDomain<PaymentDocumentLog>();
            this.paymentDocumentAccountLogDomain = this.container.ResolveDomain<PaymentDocumentAccountLog>();
        }

        protected string GetFilesPath(IPeriod period, string path)
        {
            var rootPath = BaseDocumentExecutor.GetRootPath(period);
            var filesPath = Path.Combine(rootPath, path);

            return filesPath;
        }

        protected void SavePaymentDocumentLog(
            string uid, 
            string path, 
            IEnumerable<PaymentDocumentSnapshot> snapshots, 
            IEnumerable<AccountPaymentInfoSnapshot> accountInfoSnapshots = null)
        {
            using (this.container.Using(this.paymentDocumentLogDomain))
            {
                List<long> accountIds;

                if (accountInfoSnapshots == null)
                {
                    accountIds = snapshots.Select(x => x.HolderId).ToList();
                }
                else
                {
                    accountIds = accountInfoSnapshots.Select(x => x.AccountId).ToList();                  
                }
                
                var parent = this.paymentDocumentLogDomain.GetAll().First(x => x.Uid == uid && x.Parent == null);
                var paymentDocumentLog = new PaymentDocumentLog
                {
                    Uid = uid,
                    Description = path,
                    Count = accountIds.Count,
                    Parent = parent,
                    Period = parent.Period
                };

                var accountLogs = new List<PaymentDocumentAccountLog>();
                foreach (var accountId in accountIds)
                {
                    accountLogs.Add(new PaymentDocumentAccountLog
                    {
                        Log = paymentDocumentLog,
                        AccountId = accountId
                    });
                }
                this.paymentDocumentLogDomain.Save(paymentDocumentLog);
                accountLogs.ForEach(this.paymentDocumentAccountLogDomain.Save);
            }
        }

        /// <summary>
        /// Добавить в системную группу "Сформирован документ в открытом периоде"
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="primarySources">По каким данным печатать квитанцию</param>
        protected void AddToGroupInOpenPeriod(IPeriod period, List<PayDocPrimarySource> primarySources)
        {
            var groupManager = this.container.Resolve<IGroupManager>("FormedInOpenPeriodSystemGroup");
            try
            {
                groupManager.AddToGroupWithCheckPeriod(period, primarySources.Select(x => x.AccountId).ToList());
            }
            finally
            {
                this.container.Release(groupManager);
            }
        }

        internal void GenerateReport(
            PaymentDocumentConfigContainer options,
            PaymentDocReportManager reportManager,
            ReportArgs args,
            ICodedReport report,
            ChargePeriod period,
            bool isPartially = false)
        {
            var tplCopyRepo = this.container.ResolveRepository<PaymentDocumentTemplate>();
            using (this.container.Using(tplCopyRepo))
            {
                var ext = BaseDocumentExecutor.GetReportFormat(options.PaymentDocumentConfigCommon.FileFormat).Extension();

                BaseDocumentExecutor.GenerateStimulReport(
                    reportManager,
                    report,
                    "{0}.{1}".FormatUsing(args.FileName, ext),
                    args.DocumentsPath,
                    options,
                    period,
                    isPartially);
            }
        }
        
        public abstract IDataResult Execute(BaseParams @params,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct);

        public abstract string ExecutorCode { get; }
        
        private static string GetRootPath(IPeriod period)
        {
            var config = ApplicationContext.Current.Configuration;
            return Path.Combine(config.AppSettings.GetAs<string>("FtpPath"), period.Name);
        }

        private static void GenerateStimulReport(
            PaymentDocReportManager generator,
            ICodedReport report,
            string filename,
            string path,
            PaymentDocumentConfigContainer options,
            ChargePeriod period,
            bool isPartially = false)
        {
            if (!Directory.Exists(path) || isPartially)
            {
                Directory.CreateDirectory(path);
            }

            path = path.GetCorrectPath(filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (var stream = generator.GenerateReport(report, BaseDocumentExecutor.GetReportFormat(options.PaymentDocumentConfigCommon.FileFormat), period))
            using (var file = File.Create(path))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(file);
            }
        }

        private static ReportPrintFormat GetReportFormat(FileFormat format)
        {
            if (format == FileFormat.Pdf)
            {
                return ReportPrintFormat.pdf;
            }
            if (format == FileFormat.Xps)
            {
                return ReportPrintFormat.xps;
            }

            return ReportPrintFormat.pdf;
        }
    }
}