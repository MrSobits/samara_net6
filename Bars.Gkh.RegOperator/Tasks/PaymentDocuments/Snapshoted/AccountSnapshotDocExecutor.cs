namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.Snapshoted
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.RegOperator.CodedReports.PayDoc;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Report.ReportManager;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2;
    using Castle.Windsor;
    using NHibernate.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    internal class AccountSnapshotDocExecutor : BaseDocumentExecutor
    {
        private readonly IRepository<PaymentDocumentSnapshot> snapshotDomain;
        private readonly IRepository<PaymentDocumentTemplate> templateCopyRepo;
        private readonly RegOperatorConfig config;

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public AccountSnapshotDocExecutor(
            IWindsorContainer container,
            IRepository<PaymentDocumentSnapshot> snapshotDomain,
            IRepository<PaymentDocumentTemplate> templateCopyRepo)
            : base(container)
        {
            this.snapshotDomain = snapshotDomain;
            this.templateCopyRepo = templateCopyRepo;

            var confProvider = container.Resolve<IGkhConfigProvider>();
            this.config = confProvider.Get<RegOperatorConfig>();
        }

        public override IDataResult Execute(
            BaseParams @params, 
            ExecutionContext ctx, 
            IProgressIndicator indicator, 
            CancellationToken ct)
        {
            var ids = @params.Params.GetAs<List<long>>("ids");

            var data = this.snapshotDomain.GetAll().Fetch(x => x.Period).Where(x => ids.Contains(x.Id)).ToList();

            if (data.Any())
            {
                using (var reportMan = new PaymentDocReportManager(this.templateCopyRepo))
                {
                    var path = Path.Combine("ФИЗИЧЕСКИЕ ЛИЦА", DateTime.Now.ToString("s").GetCorrectPath());

                    foreach (var snap in data)
                    {
                        var args = new ReportArgs
                        {
                            Snapshots = new[] {snap},
                            FileName = snap.Id.ToString(),
                            DocumentsPath = this.GetFilesPath(snap.Period, path)
                        };

                        this.GenerateReport(
                            this.config.PaymentDocumentConfigContainer, 
                            reportMan, 
                            args, 
                            new BaseInvoiceReport(new[] { snap }),
                            snap.Period);
                    }
                }
            }

            return new BaseDataResult();
        }

        public override string ExecutorCode { get { return "AccountSnapshotDocExecutor"; } }
    }
}