namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.Snapshoted
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using B4.Config;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Caching.LinqExtensions;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Castle.Windsor;
    using V2;

    internal class SnapshotDocTaskProvider : ITaskProvider
    {
        private readonly IWindsorContainer _container;

        public SnapshotDocTaskProvider(IWindsorContainer container)
        {
            _container = container;
        }

        #region Implementation of ITaskProvider

        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            var loadParam = @params.GetLoadParam();
            var ids = @params.Params.GetAs<List<long>>("ids");
            var periodId = @params.Params.GetAsId("periodId");

            var dmn = _container.ResolveDomain<PaymentDocumentSnapshot>();
            var chargeDmn = _container.ResolveDomain<ChargePeriod>();
            var configProv = _container.Resolve<IConfigProvider>();

            var ftpRoot = configProv.GetConfig().AppSettings.GetAs<string>("FtpPath");
            var logFilePath = Path.Combine(ftpRoot, DateTime.Now.Ticks.ToString());

            using (_container.Using(dmn, chargeDmn))
            {
                var period = chargeDmn.Get(periodId);

                if (period == null)
                {
                    throw new InvalidOperationException("Нет информации по периоду начисления!");
                }

                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }

                var snapshots = dmn.GetAll()
                    .WhereIf(ids.IsNotEmpty(), x => ids.Contains(x.Id))
                    .Filter(loadParam, _container);

                return new CreateTasksResult(CreateOwnerTasks(snapshots, periodId, logFilePath).Union(CreateAccountTasks(snapshots, periodId, logFilePath)).ToArray());
            }
        }

        public string TaskCode { get { return "SnapshotDocTaskProvider"; } }

        #endregion

        private TaskDescriptor[] CreateAccountTasks(IQueryable<PaymentDocumentSnapshot> snapshots, long periodId, string logFilesPath)
        {
            var ids = snapshots
                .Where(x => x.HolderType == PaymentDocumentData.AccountHolderType)
                .Where(x => x.Period.Id == periodId)
                .OrderBy(x => x.Id)
                .Select(x => x.Id)
                .ToList();

            return ids.Split(100).Select(x => CreateTask(x, AccountSnapshotDocExecutor.Id, logFilesPath)).ToArray();
        }

        private TaskDescriptor[] CreateOwnerTasks(IQueryable<PaymentDocumentSnapshot> snapshots, long periodId, string logFilesPath)
        {
            var ids = snapshots
                .Where(x => x.HolderType == PaymentDocumentData.OwnerholderType)
                .Where(x => x.Period.Id == periodId)
                .OrderBy(x => x.Id)
                .Select(x => x.Id)
                .ToList();

            return ids.Split(100).Select(x => CreateTask(x, OwnerSnapshotDocExecutor.Id, logFilesPath)).ToArray();
        }

        private TaskDescriptor CreateTask(IEnumerable<long> ids, string executorCode, string logFilesPath)
        {
            var prms = new BaseParams
            {
                Params = DynamicDictionary.Create()
                    .SetValue("ids", ids)
                    .SetValue("logFilesPath", logFilesPath)
            };

            return new TaskDescriptor("Формирование документов на оплату", executorCode, prms)
            {
                SuccessCallback = LogFileMergeCallback.Id,
                FailCallback = LogFileMergeCallback.Id
            }; 
        }
    }
}