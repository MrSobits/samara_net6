namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;
    using Bars.Gkh.Extensions;

    using Dapper;

    /// <summary>
    /// Действие заполнения истории принадлежности лс абоненту
    /// </summary>
    [Repeatable]
    public class FillOwnershipHistoryAction : BaseExecutionAction
    {
        private const string Query = @"
BEGIN;

TRUNCATE REGOP_PERS_ACC_OWNERSHIP_HISTORY;

--1. заполнение текущей истории
insert into REGOP_PERS_ACC_OWNERSHIP_HISTORY
(
	object_version, object_create_date, object_edit_date,
	ACTUAL_FROM, DATE, 
	ACCOUNT_ID, OWNER_ID
)
select 0, now()::date, now()::date,
	ACTUAL_FROM, DATE,
	ACC_ID AS ACCOUNT_ID, o.ID AS OWNER_ID
from REGOP_PERS_ACC_CHANGE ch
join REGOP_PERS_ACC_OWNER o on o.id = CAST(coalesce(new_value, '0') AS bigint) 
where CHANGE_TYPE = 4
    and OLD_VALUE ~ E'^\\d+$'
    and NEW_VALUE ~ E'^\\d+$';	
	
--2. заполнение первого абонента	
insert into REGOP_PERS_ACC_OWNERSHIP_HISTORY
(
	object_version, object_create_date, object_edit_date,
	ACTUAL_FROM, DATE, 
	ACCOUNT_ID, OWNER_ID
)
select 0, now()::date, now()::date,
	res.ACTUAL_FROM, res.ACTUAL_FROM as DATE,
	res.ACCOUNT_ID, o.ID AS OWNER_ID
from 
REGOP_PERS_ACC_CHANGE ch 
join REGOP_PERS_ACC_OWNER o on o.id = CAST(coalesce(ch.old_value, '0') AS bigint) 
join 
(
	select min(ch.id) as first_rec, pa.id as ACCOUNT_ID,
		(select min(cstart) from regop_period)as ACTUAL_FROM
	from REGOP_PERS_ACC_CHANGE ch
	join REGOP_PERS_ACC pa on pa.id = ACC_ID
	where CHANGE_TYPE = 4
        and OLD_VALUE ~ E'^\\d+$'
        and NEW_VALUE ~ E'^\\d+$'
	group by pa.id, pa.OPEN_DATE
) res on res.first_rec = ch.id
where CHANGE_TYPE = 4
    and OLD_VALUE ~ E'^\\d+$'
    and NEW_VALUE ~ E'^\\d+$';

	
--3. заполнение истории лс, которые никогда не меняли владельца
insert into REGOP_PERS_ACC_OWNERSHIP_HISTORY
(
	object_version, object_create_date, object_edit_date,
	ACTUAL_FROM, DATE, 
	ACCOUNT_ID, OWNER_ID
)
select 0, now()::date, now()::date,
	OPEN_DATE as ACTUAL_FROM, OPEN_DATE as DATE,
	pa.id as ACCOUNT_ID, ACC_OWNER_ID AS OWNER_ID
from 
REGOP_PERS_ACC pa
where not exists
(
	select 1
	from REGOP_PERS_ACC_CHANGE
	where ACC_ID = pa.id
	and CHANGE_TYPE = 4
    and OLD_VALUE ~ E'^\\d+$'
    and NEW_VALUE ~ E'^\\d+$'
);

DROP INDEX IF EXISTS ind_regop_ownership_account;
DROP INDEX IF EXISTS ind_regop_ownership_owner;

CREATE INDEX ind_regop_ownership_account ON REGOP_PERS_ACC_OWNERSHIP_HISTORY (ACCOUNT_ID);
CREATE INDEX ind_regop_ownership_owner ON REGOP_PERS_ACC_OWNERSHIP_HISTORY (OWNER_ID);
ANALYZE public.REGOP_PERS_ACC_OWNERSHIP_HISTORY;

update  regop_individual_acc_own o set ro_id = q.ro_id
from(
	select riao.id, gro.id ro_id 
	from
	regop_individual_acc_own riao 
	join b4_fias_address bfa on riao.fias_fact_address_id = bfa.id
	left join b4_fias_address bfa2 on bfa2.street_guid::varchar = bfa.street_guid::varchar 
									and bfa2.house::varchar = bfa.house::varchar
									and (coalesce(bfa2.housing::varchar,'') = coalesce(bfa.housing::varchar,''))
									and (coalesce(bfa2.letter::varchar,'') = coalesce(bfa.letter::varchar,''))
									and (coalesce(bfa2.building::varchar,'') = coalesce(bfa.building::varchar,''))
									and ((bfa2.flat is null) or bfa2.flat ='') 
	join gkh_reality_object gro ON gro.fias_address_id = bfa2.id
	where riao.fias_fact_address_id is not null
) q
where q.id = o.id;

update  regop_individual_acc_own  o set ro_id = null 
where o.fias_fact_address_id is null;

END;";

        /// <summary>
        /// Код действия
        /// </summary>
        public static string ActionCode = "FillOwnershipHistoryAction";

        public override string Code => FillOwnershipHistoryAction.ActionCode;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Заполнение истории принадлежности лс абоненту";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "РегОператор - Заполнение истории принадлежности лс абоненту";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            this.Container.Resolve<ISessionProvider>().InStatelessConnectionTransaction(
                (connection, transaction) => { connection.Execute(FillOwnershipHistoryAction.Query, transaction: transaction, commandTimeout: 10000); });

            return new BaseDataResult();
        }
    }
}