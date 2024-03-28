namespace Bars.Gkh.RegOperator.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;

    using Bars.Gkh.Entities;

    using Entities;
    using NHibernate;

    /// <summary>
    /// Действие для обновления сальдо
    /// </summary>
    public class UpdateSaldoSqlAction
    {
        private readonly IStatelessSession session;

        /// <summary>
        /// .ctor
        /// </summary>
        public UpdateSaldoSqlAction(IStatelessSession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Обновить сальдо 
        /// </summary>
        public void UpdatePaSaldos(ChargePeriod[] periods, IEnumerable<long> accountIds = null, bool updateSaldoIn =true)
        {
            var ids = accountIds != null ? accountIds.ToArray() : null;

            if (updateSaldoIn)
            {
                foreach (var chargePeriod in periods.OrderBy(x => x.StartDate))
                {

                    var query1 = this.session.CreateSQLQuery(string.Format(@"
                        update regop_pers_acc_period_summ ps
                        set 
                            SALDO_IN=coalesce(
                                (select saldo_out
                                from regop_pers_acc_period_summ ps1 
                                    join regop_period p1 on p1.id = ps1.period_id
                                where p1.cstart < p.cstart and ps1.account_id = ps.account_id
                                order by p1.cstart desc
                                limit 1), 0)
                        from regop_period p
                        where p.id = ps.period_id and ps.period_id=:periodId {0};",
                        ids.IsEmpty()
                            ? ""
                            : "and ps.account_id in (:accountIds)"))
                        .SetInt64("periodId", chargePeriod.Id);

                    if (!ids.IsEmpty())
                    {
                        query1 = query1.SetParameterList("accountIds", ids);
                    }

                    query1.ExecuteUpdate();

                    var query2 = this.session.CreateSQLQuery(string.Format(@"
                        update regop_pers_acc_period_summ ps
                        set
                            SALDO_OUT=SALDO_IN + CHARGE_TARIFF + PENALTY 
                                + RECALC + RECALC_DECISION + RECALC_PENALTY - PERF_WORK_CHARGE
                                + BALANCE_CHANGE + DEC_BALANCE_CHANGE + PENALTY_BALANCE_CHANGE
                                - (TARIFF_PAYMENT + TARIFF_DESICION_PAYMENT + PENALTY_PAYMENT)
                        where ps.period_id=:periodId {0};",
                        ids.IsEmpty()
                            ? ""
                            : "and ps.account_id in (:accountIds)"))
                        .SetInt64("periodId", chargePeriod.Id);

                    if (!ids.IsEmpty())
                    {
                        query2 = query2.SetParameterList("accountIds", ids);
                    }

                    query2.ExecuteUpdate();
                }
            }
            IQuery query3 = this.session.CreateSQLQuery(string.Format(@"
                    update regop_pers_acc pa
                    set
                        tariff_charge_balance = coalesce(wallet.tariff_balance, 0),
                        decision_charge_balance = coalesce(wallet.decision_balance, 0),
                        penalty_charge_balance = coalesce(wallet.penalty_balance, 0)
                    from (
                            select
                            A.ID AS account_id,
                            sum(s.charge_base_tariff - tariff_payment + s.recalc) AS tariff_balance,
                            sum(s.charge_tariff - s.charge_base_tariff + recalc_decision - tariff_desicion_payment) AS decision_balance,
                            sum(s.penalty + s.recalc_penalty - s.penalty_payment) AS penalty_balance
                            from regop_pers_acc A
                            join regop_pers_acc_period_summ s ON A.ID = s.account_id
                            group by A.ID
                        ) as wallet
                    where pa.ID = wallet.account_id {0};", ids.IsEmpty()
                ? ""
                : "and pa.id in (:accountIds)"));


            if (!ids.IsEmpty())
            {
                query3 = query3.SetParameterList("accountIds", ids);
            }

            query3.ExecuteUpdate();
        }

        /// <summary>
        /// Обновить сальдо у домов
        /// </summary>
        public void UpdateRoSaldos(ChargePeriod[] periods, IEnumerable<long> roIds = null)
        {
            var ids = roIds != null ? roIds.ToArray() : null;

            foreach (var chargePeriod in periods)
            {
                var query1 = this.session.CreateSQLQuery(string.Format(@"
                        update regop_ro_charge_acc_charge cac
                        set
                            ccharged = coalesce(tt.ccharged, 0),
                            cpaid = coalesce(tt.cpaid, 0),
                            ccharged_penalty = coalesce(tt.ccharged_penalty, 0),
                            cpaid_penalty = coalesce(tt.cpaid_penalty, 0),
                            csaldo_in = coalesce(tt.csaldo_in, 0),
                            csaldo_out = coalesce(tt.csaldo_out, 0),
                            balance_change = coalesce(tt.balance_change, 0)
                        from (
                            select
                                ca.id as ca_id,
                                ca.ro_id,
                                ps.period_id,
                                sum(ps.charge_tariff + ps.recalc + ps.balance_change + ps.recalc_decision + ps.penalty + ps.recalc_penalty) as ccharged,
                                sum(ps.tariff_payment + ps.tariff_desicion_payment) as cpaid,
                                sum(ps.penalty + ps.recalc_penalty) as ccharged_penalty,
                                sum(ps.penalty_payment) as cpaid_penalty,
                                sum(ps.saldo_in) as csaldo_in,
                                sum(ps.saldo_out) as csaldo_out,
                                sum(ps.balance_change) as balance_change
                            from regop_pers_acc_period_summ ps
                                join regop_pers_acc pa on pa.id = ps.account_id
                                join gkh_room r on r.id = pa.room_id
                                join regop_ro_charge_account ca on ca.ro_id=r.ro_id
                            group by ca.id, ps.period_id, ca.ro_id
                        ) tt
                        where cac.acc_id = tt.ca_id
                            and cac.period_id = tt.period_id
                            and tt.period_id=:periodId {0};",
                            ids.IsEmpty() 
                                ? "" 
                                : "and tt.ro_id in (:roIds)"))

                    .SetInt64("periodId", chargePeriod.Id);

                if (!ids.IsEmpty())
                {
                    query1 = query1.SetParameterList("roIds", ids);
                }
                    
                query1.ExecuteUpdate();
            }

            IQuery query2 = this.session.CreateSQLQuery(string.Format(@"
                update regop_ro_charge_account ca
                set
                    charge_total = coalesce(tt.charge_total, 0),
                    paid_total = coalesce(tt.paid_total, 0)
                from (
                    select
                        cac.acc_id,
                        sum(ccharged + ccharged_penalty) as charge_total,
                        sum(cpaid + cpaid_penalty) as paid_total
                    from regop_ro_charge_acc_charge cac
                    group by cac.acc_id
                ) tt
                where tt.acc_id = ca.id {0};", roIds.IsEmpty() ? "" : "and ca.ro_id in (:roIds)"));

            if (!ids.IsEmpty())
            {
                query2 = query2.SetParameterList("roIds", ids);
            }

            query2.ExecuteUpdate();
        }
    }
}