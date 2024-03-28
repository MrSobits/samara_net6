namespace Bars.Gkh.RegOperator.Tasks.CorrectTransfers.OrigName
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using B4;
    using B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using B4.Modules.Tasks.Common.Service;
    using B4.Modules.Tasks.Common.Utils;

    using Bars.B4.Utils;

    using Entities;
    using ServiceStack.Common;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Исправление OriginatorName у трансферов оплат лицевый счетов
    /// </summary>
    public class CorrectPaymentOriginatorNameTaskExecutor : ITaskExecutor
    {
        /// <summary>
        /// 
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly ISessionProvider _sessionProvider;
        private readonly IDomainService<BasePersonalAccount> _accountDomain;
        private readonly ILogger _logManager;

        /// <summary>
        /// .ctor
        /// </summary>
        public CorrectPaymentOriginatorNameTaskExecutor(
            ISessionProvider sessionProvider,
            IDomainService<BasePersonalAccount> accountDomain,
            ILogger logManager)
        {
            _sessionProvider = sessionProvider;
            _accountDomain = accountDomain;
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
            var persaccs = _accountDomain.GetAll()
                .Select(x => x.Id)
                .ToArray();

            const int take = 100;

            for (int i = 0; i < persaccs.Length; i += take)
            {
                if (_token.IsCancellationRequested)
                {
                    break;
                }

                for (int tryCount = 1; tryCount < 4; tryCount++)
                {
                    if (_token.IsCancellationRequested)
                    {
                        break;
                    }

                    int to = Math.Min(i + take, persaccs.Length);

                    var portion = persaccs.Skip(i).Take(take).ToArray();

                    try
                    {
                        ProcessPortion(portion);

                        Indicate(i, take, persaccs.Length);

                        _logManager.LogDebug(
                            string.Format("Обработана пачка лицевых счетов с {0} по {1}",
                                i, to));
                    }
                    catch (Exception)
                    {
                        //пробуем обработать порцию 3 раза, затем переходим к следующей
                        if (tryCount < 4)
                        {
                            _logManager.LogDebug(
                                string.Format("Ошибка обработки пачки лицевых счетов с {0} по {1}, попытка {2}",
                                    i, to, tryCount));

                            continue;
                        }
                        else
                        {
                            Indicate(i, take, persaccs.Length);

                            _logManager.LogDebug(
                                string.Format("Ошибка обработки пачки лицевых счетов с {0} по {1}, пропускается :(",
                                    i, to));
                        }
                    }
                    finally
                    {
                        _sessionProvider.GetCurrentSession().Clear();
                    }

                    break;
                }
            }
        }

        private void Indicate(int i, int take, int length)
        {
            if(_indicator == null) return;

            var processed = Math.Min(i + take, length);

            var percent = processed * 100m / length;

            _indicator.Indicate(null, (uint)percent, string.Format("Обработано {0} из {1} лицевых счетов", processed, length));
        }

        private void ProcessPortion(long[] accIds)
        {
            if(accIds.IsEmpty()) return;

            using (var ss = _sessionProvider.OpenStatelessSession())
            {
                foreach (var walletName in _walletNames)
                {
                    using (var tr = ss.BeginTransaction())
                    {
                        try
                        {
                            var query1 = string.Format(@"
update regop_transfer t set originator_name = pa.acc_num
from regop_wallet w
	join regop_pers_acc pa on pa.{0} = w.id
where w.wallet_guid = t.target_guid and pa.id in (:ids)
    and t.originator_name is null", walletName);

                            var query3 = string.Format(@"
update regop_transfer t set originator_name = pa.acc_num
from regop_wallet w
	join regop_pers_acc pa on pa.{0} = w.id
where w.wallet_guid = t.source_guid and pa.id in (:ids)
    and t.originator_name is null and t.reason not like 'Начисление%'", walletName);

                            var query2 = string.Format(@"
update regop_transfer t set originator_name = tt.originator_name
from regop_transfer tt
	join regop_wallet w on w.wallet_guid = tt.target_guid
	join regop_pers_acc pa on pa.{0} = w.id
where tt.id = t.originator_id and tt.originator_name is not null and pa.id in (:ids)
    and t.originator_name is null", walletName);

                            var query4 = string.Format(@"
update regop_transfer t set originator_name = tt.originator_name
from regop_transfer tt
	join regop_wallet w on w.wallet_guid = tt.source_guid
	join regop_pers_acc pa on pa.{0} = w.id
where tt.id = t.originator_id and tt.originator_name is not null and pa.id in (:ids)
    and t.originator_name is null and t.reason not like 'Начисление%'", walletName);

                            ss.CreateSQLQuery(query1).SetParameterList("ids", accIds).ExecuteUpdate();
                            ss.CreateSQLQuery(query3).SetParameterList("ids", accIds).ExecuteUpdate();
                            ss.CreateSQLQuery(query2).SetParameterList("ids", accIds).ExecuteUpdate();
                            ss.CreateSQLQuery(query4).SetParameterList("ids", accIds).ExecuteUpdate();

                            tr.Commit();
                        }
                        catch (Exception)
                        {
                            if(tr.IsActive) tr.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// код исполнителя
        /// </summary>
        public string ExecutorCode { get; private set; }

        private readonly string[] _walletNames =
        {
            "BT_WALLET_ID",
            "DT_WALLET_ID",
            "AF_WALLET_ID",
            "P_WALLET_ID",
            "PWP_WALLET_ID",
            "R_WALLET_ID",
            "SS_WALLET_ID",
            "RAA_WALLET_ID"
        };
    }
}
