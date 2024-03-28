namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.Snapshoted
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.RegOperator.CodedReports;
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

    internal class OwnerSnapshotDocExecutor : BaseDocumentExecutor
    {
        private readonly IRepository<PaymentDocumentSnapshot> snapshotRepo;
        private readonly IRepository<AccountPaymentInfoSnapshot> accountSnapshotRepo;
        private readonly IRepository<PaymentDocumentTemplate> templateCopyRepo;
        private readonly RegOperatorConfig config;

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public OwnerSnapshotDocExecutor(
            IWindsorContainer container,
            IRepository<PaymentDocumentSnapshot> snapshotRepo,
            IRepository<AccountPaymentInfoSnapshot> accountSnapshotRepo,
            IRepository<PaymentDocumentTemplate> templateCopyRepo)
            : base(container)
        {
            this.snapshotRepo = snapshotRepo;
            this.accountSnapshotRepo = accountSnapshotRepo;
            this.templateCopyRepo = templateCopyRepo;

            var confProvider = container.Resolve<IGkhConfigProvider>();
            this.config = confProvider.Get<RegOperatorConfig>();
        }

        public override IDataResult Execute(BaseParams @params,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            var ids = @params.Params.GetAs<List<long>>("ids");

            var snapshots = this.snapshotRepo.GetAll().Where(x => ids.Contains(x.Id));
            var accountInfo = this.accountSnapshotRepo.GetAll().Where(x => snapshots.Any(s => s == x.Snapshot));

            if (snapshots.Any())
            {
                using (var reportMan = new PaymentDocReportManager(this.templateCopyRepo))
                {
                    var path = Path.Combine("ЮРИДИЧЕСКИЕ ЛИЦА", DateTime.Now.ToString("s").GetCorrectPath());

                    var snapshotsWithPeriod = snapshots.Fetch(x => x.Period);

                    foreach (var snapshot in snapshotsWithPeriod)
                    {
                        var args = new ReportArgs
                        {
                            Snapshots = new[] {snapshot},
                            AccountInfos = accountInfo.Where(x => x.Snapshot.Id == snapshot.Id).ToList(),
                            FileName = snapshot.Id.ToString(),
                            DocumentsPath = this.GetFilesPath(snapshot.Period, path)
                        };

                        this.GenerateReport(
                            this.config.PaymentDocumentConfigContainer,
                            reportMan,
                            args,
                            new InvoiceRegistryAndActReport(args.Snapshots, args.AccountInfos),
                            snapshot.Period);
                    }
                }
            }

            return new BaseDataResult();
        }

        public override string ExecutorCode
        {
            get { return "OwnerSnapshotDocExecutor"; }
        }
    }
}