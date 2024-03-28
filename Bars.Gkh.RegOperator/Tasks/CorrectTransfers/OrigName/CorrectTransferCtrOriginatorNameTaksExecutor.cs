namespace Bars.Gkh.RegOperator.Tasks.CorrectTransfers.OrigName
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using B4;
    using B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using B4.Modules.Tasks.Common.Service;
    using B4.Modules.Tasks.Common.Utils;

    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    using Castle.Windsor;
    using Entities;
    using Entities.ValueObjects;
    using Gkh.Domain;
    using Gkh.Utils;
    using NHibernate.Linq;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Исправление OriginatorName у трансферов оплат подрядчикам
    /// </summary>
    public class CorrectTransferCtrOriginatorNameTaksExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer _container;
        private readonly ISessionProvider _sessionProvider;
        private readonly IDomainService<RealityObjectTransfer> _transferDomain;
        private readonly IDomainService<TransferCtr> _transferCtrDomain;
        private readonly ILogger _logManager;

        /// <summary>
        /// .ctor
        /// </summary>
        public CorrectTransferCtrOriginatorNameTaksExecutor(
            IWindsorContainer container,
            ISessionProvider sessionProvider,
            IDomainService<RealityObjectTransfer> transferDomain,
            IDomainService<TransferCtr> transferCtrDomain,
            ILogger logManager)
        {
            _container = container;
            _sessionProvider = sessionProvider;
            _transferDomain = transferDomain;
            _transferCtrDomain = transferCtrDomain;
            _logManager = logManager;
        }

        private IProgressIndicator _indicator;
        private CancellationToken _token;

        /// <summary>
        /// Выполнить задачу
        /// </summary>
        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            _indicator = indicator;
            _token = ct;

            try
            {
                Execute();
            }
            catch (Exception e)
            {
                return BaseDataResult.Error(e.Message);
            }

            return new BaseDataResult();
        }

        private void Execute()
        {
            var transfersCtr = _transferCtrDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.TransferGuid,
                    x.DateFrom,
                    x.DocumentNum
                })
                .AsEnumerable()
                .Select(x => new Tuple<string, string>(
                    x.TransferGuid,
                    string.Format("№ {0} от {1}", x.DocumentNum, x.DateFrom.ToDateString())))
                .ToArray();

            const int take = 1000;
            int retryCount = 0;

            for (int i = 0; i < transfersCtr.Length; i += take)
            {
                if (_token.IsCancellationRequested)
                {
                    break;
                }

                int to = Math.Min(i + take, transfersCtr.Length);

                var portion = transfersCtr.Skip(0).Take(take).ToArray();

                try
                {
                    ProcessPortion(portion);
                    retryCount = 0;

                    Indicate(i, take, transfersCtr.Length);

                    _logManager.LogDebug(
                        string.Format("Обработана пачка оплат подрядчикам с {0} по {1}",
                            i, to));
                }
                catch (Exception)
                {
                    if (retryCount < 3)
                    {
                        retryCount++;
                        i -= take;

                        _logManager.LogDebug(
                            string.Format("Ошибка обработки пачки оплат подрядчикам с {0} по {1}, попытка {2}",
                                i, to - take, retryCount));
                    }
                    else
                    {
                        retryCount = 0;
                        Indicate(i, take, transfersCtr.Length);

                        _logManager.LogDebug(
                            string.Format("Ошибка обработки пачки оплат подрядчикам с {0} по {1}, пропускается :(",
                                i, to));
                    }
                }
                finally
                {
                    _sessionProvider.GetCurrentSession().Clear();
                }
            }
        }

        private void ProcessPortion(Tuple<string, string>[] portion)
        {
            var portionGuids = portion.Select(x => x.Item2).ToArray();

            var transfers = GetTransferByGuids(portionGuids);

            var toSave = new List<BaseEntity>();

            foreach (var transfer in transfers)
            {
                foreach (var payment in portion)
                {
                    var guid = payment.Item2;
                    var name = payment.Item1;

                    if (guid == transfer.TargetGuid || guid == transfer.SourceGuid)
                    {
                        transfer.OriginatorName = name;
                        toSave.Add(transfer);
                        break;
                    }

                    if (transfer.Originator != null)
                    {
                        var origTransfer = transfer.Originator;

                        if (guid == origTransfer.TargetGuid || guid == origTransfer.SourceGuid)
                        {
                            transfer.OriginatorName = name;
                            toSave.Add(transfer);
                            break;
                        }
                    }
                }
            }

            TransactionHelper.InsertInManyTransactions(_container, toSave, toSave.Count, false, true);
        }

        private void Indicate(int i, int take, int length)
        {
            var processed = Math.Min(i + take, length);

            var percent = processed * 100m / length;

            _indicator.Indicate(null, (uint)percent, string.Format("Обработано {0} из {1} актов выполненных работ", processed, length));
        }

        private IEnumerable<Transfer> GetTransferByGuids(string[] guids)
        {
            return _transferDomain.GetAll()
                .Fetch(x => x.Originator)
                .Where(x => x.OriginatorName == null || x.OriginatorName == "")
                .Where(x => guids.Contains(x.TargetGuid)
                    || (guids.Contains(x.SourceGuid) && x.CopyTransfer != null)
                    || guids.Contains(x.Originator.TargetGuid))
                .ToArray();
        }

        /// <summary>
        /// Код исполнителя
        /// </summary>
        public string ExecutorCode { get; private set; }
    }
}