namespace Bars.Gkh.RegOperator.Tasks.Period.Executors
{
    using System;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Service;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Закрытие периода (Этап 2)
    /// </summary>
    public class PeriodCloseTaskExecutor_Step2 : ITaskExecutor
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;
        private readonly IWindsorContainer container;
        private readonly ISessionProvider sessionProvider;

        /// <summary>
        /// .ctor
        /// </summary>
        public PeriodCloseTaskExecutor_Step2(IWindsorContainer container, ISessionProvider sessionProvider)
        {
            this.container = container;
            this.sessionProvider = sessionProvider;
        }

        #region Implementation of ITaskExecutor
        /// <summary>
        /// Выполнить
        /// </summary>
        /// <param name="params">Параметры</param>
        /// <param name="ctx">Контекст</param>
        /// <param name="indicator">Индикатор</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Результат</returns>
        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            using (this.container.Using(this.sessionProvider))
            {
                var session = this.sessionProvider.GetCurrentSession();

                var periodId = session.CreateSQLQuery("SELECT CAST(id AS BIGINT) FROM public.regop_period WHERE cis_closed = false").UniqueResult<long>();

                string updateRoSummaryQuery = $@"update regop_ro_charge_acc_charge cac
                                        set
                                         ccharged = coalesce(tt.ccharged, 0),
                                         cpaid = coalesce(tt.cpaid, 0),
                                         ccharged_penalty = coalesce(tt.ccharged_penalty, 0),
                                         cpaid_penalty = coalesce(tt.cpaid_penalty, 0),
                                         csaldo_in = coalesce(tt.csaldo_in, 0),
                                         csaldo_out = coalesce(tt.csaldo_out, 0),
                                         balance_change = coalesce(tt.balance_change, 0)
                                        from(
                                         select
                                         ca.id as ca_id,
                                         ps.period_id,
                                         sum(ps.charge_tariff + ps.recalc + ps.recalc_decision + ps.balance_change + ps.dec_balance_change + ps.penalty_balance_change 
                                             + ps.penalty + ps.recalc_penalty - ps.perf_work_charge) as ccharged,
                                         sum(ps.tariff_payment + ps.tariff_desicion_payment) as cpaid,
                                         sum(ps.penalty + ps.recalc_penalty + ps.penalty_balance_change) as ccharged_penalty,
                                         sum(ps.penalty_payment) as cpaid_penalty,
                                         sum(ps.saldo_in) as csaldo_in,
                                         sum(ps.saldo_out) as csaldo_out,
                                         sum(ps.balance_change) as balance_change
                                         from regop_pers_acc_period_summ ps
                                         join regop_pers_acc pa on pa.id = ps.account_id
                                         join gkh_room r on r.id = pa.room_id
                                         join regop_ro_charge_account ca on ca.ro_id = r.ro_id                                        
                                         where ps.period_id={periodId}
                                           and exists(select 1 from REGOP_PERS_ACC_CHARGE ch where ch.PERS_ACC_ID = pa.id  and ch.period_id={periodId})
                                         group by ca.id, ps.period_id
                                        ) tt
                                        where cac.acc_id = tt.ca_id and cac.period_id = tt.period_id;";

                const string updateRoAccountQuery = @"update regop_ro_charge_account ca
                                        set
                                         charge_total = coalesce(tt.charge_total, 0),
                                         paid_total = coalesce(tt.paid_total, 0)
                                        from(
                                         select
                                         cac.acc_id,
                                         sum(ccharged + ccharged_penalty) as charge_total,
                                         sum(cpaid + cpaid_penalty) as paid_total
                                         from regop_ro_charge_acc_charge cac
                                         group by cac.acc_id
                                        ) tt
                                        where tt.acc_id = ca.id;";

                using (var tr = session.BeginTransaction())
                {
                    try
                    {
                        var query = session.CreateSQLQuery(updateRoSummaryQuery);
                        query.ExecuteUpdate();

                        query = session.CreateSQLQuery(updateRoAccountQuery);
                        query.ExecuteUpdate();

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();

                        throw;
                    }
                }

               
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Код
        /// </summary>
        public string ExecutorCode { get; private set; }
        #endregion
    }
}