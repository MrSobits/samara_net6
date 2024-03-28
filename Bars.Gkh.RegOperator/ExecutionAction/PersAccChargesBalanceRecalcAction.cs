namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction;

    /// <summary>
    /// Действие Перерасчет текущей задолженности по базовому тарифу, тарифу решения и пени
    /// </summary>
    public class PersAccChargesBalanceRecalcAction : BaseExecutionAction
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        /// <summary>
        /// Код действия
        /// </summary>
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Перерасчет текущей задолженности по базовому тарифу, тарифу решения и пени";

        /// <summary>
        /// Название действия
        /// </summary>
        public override string Name => "Перерасчет текущей задолженности по базовому тарифу, тарифу решения и пени";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var provider = this.Container.Resolve<ISessionProvider>();

            using (this.Container.Using(provider))
            {
                using (var session = provider.OpenStatelessSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            var recalcQuery = @"UPDATE REGOP_PERS_ACC ac
                                        SET 
                                        TARIFF_CHARGE_BALANCE = ps.TARIFF_CHARGE_BALANCE, 
                                        DECISION_CHARGE_BALANCE = ps.DECISION_CHARGE_BALANCE,
                                        PENALTY_CHARGE_BALANCE  = ps.PENALTY_CHARGE_BALANCE
                                        from
                                        (
                                            select 
                                            account_id,
                                            sum(CHARGE_BASE_TARIFF + BALANCE_CHANGE + RECALC + TARIFF_PAYMENT) as TARIFF_CHARGE_BALANCE,
                                            sum(CHARGE_TARIFF - CHARGE_BASE_TARIFF + RECALC_DECISION - TARIFF_DESICION_PAYMENT) as DECISION_CHARGE_BALANCE,
                                            sum(PENALTY + RECALC_PENALTY - PENALTY_PAYMENT) as PENALTY_CHARGE_BALANCE
                                            from REGOP_PERS_ACC_PERIOD_SUMM
                                            group by account_id
                                        ) ps
                                        where id = ps.account_id";

                            session.CreateSQLQuery(recalcQuery).ExecuteUpdate();

                            transaction.Commit();

                            return new BaseDataResult();
                        }
                        catch (Exception exception)
                        {
                            transaction.Rollback();
                            return new BaseDataResult(
                                false,
                                "{0}\n{1}\n{2}".FormatUsing(exception.Message, exception.GetType().FullName, exception.StackTrace));
                        }
                    }
                }
            }
        }
    }
}