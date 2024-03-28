namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;

    /// <summary>
    /// Действие пересчета количества лс у абонентов
    /// </summary>
    public class RecalcOwnerAccountsCountAction : BaseExecutionAction
    {
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Пересчитать количество ЛС у абонентов";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "РегОператор - Пересчитать количество ЛС у абонентов";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => RecalcOwnerAccountsCountAction.Execute;

        private static BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;

            var session = container.Resolve<ISessionProvider>().GetCurrentSession();

            var query = session.CreateSQLQuery(@"
                        update regop_pers_acc_owner aco
                        set total_accounts_count = c.totalcount,
                        active_accounts_count = c.activecount
                        from (
                        select ac.acc_owner_id,count(ac.id) totalcount,
                        (select count(a.id) from regop_pers_acc a 
                        where a.state_id=(select id from b4_state where start_state and type_id='gkh_regop_personal_account') 
                        and a.acc_owner_id=ac.acc_owner_id) activecount
                        from regop_pers_acc ac
                        group by ac.acc_owner_id)c
                        where aco.id=c.acc_owner_id");

            query.ExecuteUpdate();

            query = session.CreateSQLQuery(@"
                        update regop_pers_acc_owner o set total_accounts_count = 0, active_accounts_count = 0
                            where not exists(select id from regop_pers_acc a where a.acc_owner_id = o.id)");

            query.ExecuteUpdate();

            return new BaseDataResult();
        }
    }
}