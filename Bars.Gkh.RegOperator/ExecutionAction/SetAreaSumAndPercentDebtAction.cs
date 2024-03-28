namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;

    /// <summary>
    /// Рассчитать площади МКД и собираемость
    /// </summary>
    public class SetAreaSumAndPercentDebtAction : BaseExecutionAction
    {
        /// <summary>
        /// IoC
        /// </summary>
        /// <summary>
        /// Код действия
        /// </summary>
        /// <inheritdoc />
        public override string Description => @"Рассчитать площади МКД и собираемость";

        /// <inheritdoc />
        public override string Name => @"Рассчитать площади МКД и собираемость";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            using (var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession())
            {
                var updateRows = session.CreateSQLQuery(@"
                update gkh_reality_object ro
                set AREA_OWNED=a.AREA_OWNED,
                    AREA_MUNICIPAL_OWNED=a.AREA_MUNICIPAL_OWNED,
                    AREA_GOVERNMENT_OWNED=a.AREA_GOVERNMENT_OWNED,
                    AREA_LIV_NOT_LIV_MKD=a.AREA_LIV_NOT_LIV_MKD,
                    AREA_LIVING=a.AREA_LIVING,
                    AREA_NL_PREMISES=a.AREA_NL_PREMISES,
                    AREA_LIVING_OWNED=a.AREA_LIVING_OWNED,
                    PERCENT_DEBT=a.PERCENT_DEBT
                from (
                select ro_id,
                    sum((case when ownership_type=10 then carea else 0 end)) AREA_OWNED,
                    sum((case when ownership_type=30 then carea else 0 end)) AREA_MUNICIPAL_OWNED,
                    sum((case when ownership_type=40 then carea else 0 end)) AREA_GOVERNMENT_OWNED,
                    sum(carea) AREA_LIV_NOT_LIV_MKD,
                    sum((case when type=10 then carea else 0 end)) AREA_LIVING,
                    sum((case when type=20 then carea else 0 end)) AREA_NL_PREMISES,
                    sum((select coalesce(carea,0)
                        from gkh_room rm
                        where exists (select 1
                                    from regop_pers_acc ac
                                    join regop_pers_acc_owner aco on aco.id=ac.acc_owner_id and aco.owner_type=0
                                    where rm.id=ac.room_id)
                        and rm.type=10 and rm.id=r.id)) AREA_LIVING_OWNED,
                        (select case when sum(roch.ccharged) = 0 then 0 else (sum(roch.cpaid) + sum(roch.cpaid_penalty))/sum(roch.ccharged)*100 end
                        from REGOP_RO_CHARGE_ACC_CHARGE roch
                        join regop_ro_charge_account ca on ca.id=roch.acc_id
                        where ca.ro_id=r.ro_id
                        group by ca.ro_id) PERCENT_DEBT
                from gkh_room r
                group by ro_id) a
                where ro.id=a.ro_id")
                    .ExecuteUpdate();

                return new BaseDataResult(true, $"Обновлено строк: {updateRows}");
            }
        }
    }
}