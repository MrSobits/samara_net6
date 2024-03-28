namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using B4;
    using B4.Application;
    using B4.DataAccess;

    using Bars.Gkh.Entities;

    using Castle.Windsor;
    using Domain;
    using Entities;
    using NHibernate;

    //открывашка периода
    [Obsolete("только для внутреннего использования")]
    public class ChargePeriodOpenService : IDisposable
    {
        public BaseDataResult RollbackPeriod(ChargePeriod periodToRollback, ChargePeriod periodToDelete)
        {
            try
            {
                DeleteTransfers(periodToDelete);

                DeleteTransfers(periodToRollback);

                DeletePeriodRelations(periodToRollback);

                DeletePeriodRelations(periodToDelete);

                RollbackPersonalAccounts(periodToRollback, periodToDelete);

                RollbackRealityObjects(periodToRollback, periodToDelete);

                if (periodToDelete != null)
                {
                    Session.CreateSQLQuery(@"delete from regop_period where id=:id")
                        .SetInt64("id", periodToDelete.Id)
                        .ExecuteUpdate();
                }

                if (periodToRollback != null)
                {
                    Session.CreateSQLQuery(
                        @"update regop_period set is_closing=false, cis_closed=false,cend=null where id = :id")
                        .SetInt64("id", periodToRollback.Id)
                        .ExecuteUpdate();
                }
            }
            catch (Exception e)
            {
                return BaseDataResult.Error(string.Format(@"message: {0}    stacktrace: {1}", e.Message, e.StackTrace));
            }

            return new BaseDataResult();
        }

        public void Dispose()
        {
            _session.Close();
        }

        private void DeleteTransfers(ChargePeriod period)
        {
            if (period == null) return;

            // удаляем трансферы
            Session.CreateSQLQuery(@"
delete from regop_charge_transfer
where (reason = 'Начисление по базовому тарифу'
    or reason = 'Начисление по тарифу решения'
    or reason = 'Перерасчет по базовому тарифу'
    or reason = 'Перерасчет по тарифу решения'
    or reason = 'Начисление пени'
    or reason = 'Перерасчет пени')
    and operation_date >= :start_date
    and to_date(to_char(operation_date, 'dd.mm.yyyy'), 'dd.mm.yyyy') <= :end_date")
                .SetDateTime("start_date", period.StartDate)
                .SetDateTime("end_date", period.GetEndDate())
                .ExecuteUpdate();

            //удаляем операции
            Session.CreateSQLQuery(@"
delete from regop_money_operation mo
where not exists (select 1 from regop_charge_transfer t where t.op_id = mo.id)")
                .ExecuteUpdate();
        }

        private void DeletePeriodRelations(ChargePeriod period)
        {
            if (period == null) return;

            //удаление параметров расчета
            Session.CreateSQLQuery(@"
delete from regop_pers_acc_calc_param
where id in (
    select cp.id
    from regop_pers_acc_calc_param cp
        join regop_period p on p.object_create_date <= cp.object_create_date and p.object_edit_date >= cp.object_create_date
    where p.id = :periodId)")
                .SetInt64("periodId", period.Id)
                .ExecuteUpdate();

            //удаляем начисления
            Session.CreateSQLQuery(
                @"delete from regop_pers_acc_charge where charge_date >= :start_date and charge_date <= :end_date")
                .SetDateTime("start_date", period.StartDate)
                .SetDateTime("end_date", period.GetEndDate())
                .ExecuteUpdate();
        }

        private void RollbackPersonalAccounts(ChargePeriod periodToRollback, ChargePeriod periodToDelete)
        {
            if (periodToDelete != null)
            {
                //удаляем операции изменения сальдо
                Session.CreateSQLQuery(
                    @"delete from regop_summary_saldo_change where summary_id in (select id from regop_pers_acc_period_summ where period_id = :periodId)")
                    .SetInt64("periodId", periodToDelete.Id)
                    .ExecuteUpdate();

                //удаляем операции изменения пени
                Session.CreateSQLQuery(
                    @"delete from regop_penalty_change where summary_id in (select id from regop_pers_acc_period_summ where period_id = :periodId)")
                    .SetInt64("periodId", periodToDelete.Id)
                    .ExecuteUpdate();

                //удаляем periodSummary лицевых счетов
                Session.CreateSQLQuery(@"delete from regop_pers_acc_period_summ where period_id = :periodId")
                    .SetInt64("periodId", periodToDelete.Id)
                    .ExecuteUpdate();
            }

            if (periodToRollback != null)
            {
                //обнуляем начисления лс
                Session.CreateSQLQuery(@"
update regop_pers_acc_period_summ set
charge_base_tariff=0,
charge_tariff=0,
penalty=0,
recalc=0,
recalc_decision=0,
recalc_penalty=0
where period_id=:periodId")
                    .SetInt64("periodId", periodToRollback.Id)
                    .ExecuteUpdate();

                //пересчет исходящего сальдо в лс
                Session.CreateSQLQuery(@"
update regop_pers_acc_period_summ set
saldo_out = (saldo_in + balance_change - tariff_payment - penalty_payment - tariff_desicion_payment)
where period_id = :periodId")
                    .SetInt64("periodId", periodToRollback.Id)
                    .ExecuteUpdate();
            }

            //правка задолженностей лс
            Session.CreateSQLQuery(@"
update regop_pers_acc pa set
tariff_charge_balance = coalesce((select sum(charge_base_tariff + recalc + balance_change - tariff_payment) from regop_pers_acc_period_summ where account_id = pa.id), 0),
decision_charge_balance = coalesce((select sum(charge_tariff - charge_base_tariff + recalc_decision - tariff_desicion_payment) from regop_pers_acc_period_summ where account_id = pa.id), 0),
penalty_charge_balance = coalesce((select sum(penalty + recalc_penalty - penalty_payment) from regop_pers_acc_period_summ where account_id = pa.id), 0)")
                .ExecuteUpdate();
        }

        private void RollbackRealityObjects(ChargePeriod periodToRollback, ChargePeriod periodToDelete)
        {
            if (periodToDelete != null)
            {
                //удаляем periodSummary счетов начислений
                Session.CreateSQLQuery(@"delete from regop_ro_charge_acc_charge where period_id = :periodId")
                    .SetInt64("periodId", periodToDelete.Id)
                    .ExecuteUpdate();
            }

            if (periodToRollback != null)
            {
                //обнуляем начисления в доме
                Session.CreateSQLQuery(@"
update regop_ro_charge_acc_charge set
ccharged=0,
ccharged_penalty=0
where period_id = :periodId")
                    .SetInt64("periodId", periodToRollback.Id)
                    .ExecuteUpdate();

                //пересчет исходящего сальдо в лс
                Session.CreateSQLQuery(@"
update regop_ro_charge_acc_charge set
csaldo_out = (csaldo_in + balance_change - cpaid - cpaid_penalty)
where period_id = :periodId")
                    .SetInt64("periodId", periodToRollback.Id)
                    .ExecuteUpdate();
            }

            //правка задолженностей в доме
            Session.CreateSQLQuery(@"
update regop_ro_charge_account roc set
charge_total = coalesce((select sum(ccharged + ccharged_penalty + balance_change) from regop_ro_charge_acc_charge where acc_id = roc.id), 0),
paid_total = coalesce((select sum(cpaid + cpaid_penalty) from regop_ro_charge_acc_charge where acc_id = roc.id), 0)")
                .ExecuteUpdate();
        }

        private IWindsorContainer Container
        {
            get
            {
                return ApplicationContext.Current.Container;
            }
        }

        private ISession _session;

        private ISession Session
        {
            get
            {
                var s = _session ?? (_session = Container.Resolve<ISessionProvider>().GetCurrentSession());

                if (!s.IsOpen)
                {
                    _session = Container.Resolve<ISessionProvider>().CreateNewSession();
                }
                return _session;
            }
        }


    }
}