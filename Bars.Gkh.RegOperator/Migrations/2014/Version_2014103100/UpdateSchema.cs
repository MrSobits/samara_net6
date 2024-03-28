namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014103100
{
    using System;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014103100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014102902.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
//            try
//            {
//                //дебетовые незаймовые операции
//                Database.ExecuteNonQuery(@"
//create or replace view view_debet_operations as
//
//select
//	t.target_guid as guid,
//	t.amount,
//	t.operation_date
//from regop_transfer t
//	inner join regop_money_operation mo on mo.id = t.op_id
//where mo.canceled_op_id is null and not t.is_loan and t.reason <> 'Взятие займа' and t.reason <> 'Возврат займа'
//	
//union all
//
//select
//	t.source_guid as guid,
//	-1*t.amount,
//	t.operation_date
//from regop_transfer t
//	inner join regop_money_operation mo on mo.id = t.op_id
//where mo.canceled_op_id is not null and not t.is_loan and t.reason <> 'Взятие займа' and t.reason <> 'Возврат займа'");

//                //кредитовые незаймовые операции
//                Database.ExecuteNonQuery(@"
//create or replace view view_credit_operations as
//
//select
//	t.source_guid as guid,
//	t.amount,
//	t.operation_date
//from regop_transfer t
//	inner join regop_money_operation mo on mo.id = t.op_id
//where mo.canceled_op_id is null and not t.is_loan and t.reason <> 'Взятие займа' and t.reason <> 'Возврат займа'
//	
//union all
//
//select
//	t.target_guid as guid,
//	-1*t.amount,
//	t.operation_date
//from regop_transfer t
//	inner join regop_money_operation mo on mo.id = t.op_id
//where mo.canceled_op_id is not null and not t.is_loan and t.reason <> 'Взятие займа' and t.reason <> 'Возврат займа'");

//                //дебетовые займовые операции
//                Database.ExecuteNonQuery(@"
//create or replace view view_debet_loan_operations as
//
//select
//	t.target_guid as guid,
//	t.amount,
//	t.operation_date
//from regop_transfer t
//where t.reason='Возврат займа'");

//                //кредитовые займовые операции
//                Database.ExecuteNonQuery(@"
//create or replace view view_credit_loan_operations as
//
//select
//	t.source_guid as guid,
//	t.amount,
//	t.operation_date
//from regop_transfer t
//where t.reason='Взятие займа'");

//                //кредитовые займовые операции по домам
//                Database.ExecuteNonQuery(@"
//create materialized view view_robject_credit_loan_sum as
//
//select
//	tt.ro_id,
//	tt.p_id,
//	tt.bt_amount,
//	tt.dt_amount,
//	tt.p_amount,
//	tt.af_amount,
//	tt.pwp_amount,
//	tt.r_amount,
//	tt.ss_amount,
//	tt.fsu_amount,
//	tt.rsu_amount,
//	tt.ssu_amount,
//	tt.tsu_amount,
//	tt.os_amount,
//	tt.bp_amount,
//	
//	tt.bt_amount + tt.dt_amount + tt.p_amount
//		+ tt.af_amount + tt.pwp_amount + tt.r_amount
//		+ tt.ss_amount + tt.fsu_amount + tt.rsu_amount
//		+ tt.ssu_amount	+ tt.tsu_amount	+ tt.os_amount
//		+ tt.bp_amount as amount
//from (
//	select
//		ropa.ro_id,
//		per.id as p_id,
//		coalesce(sum(bt.amount), 0) as bt_amount,
//		coalesce(sum(dt.amount), 0) as dt_amount,
//		coalesce(sum(p.amount), 0) as p_amount,
//		coalesce(sum(af.amount), 0) as af_amount,
//		coalesce(sum(pwp.amount), 0) as pwp_amount,
//		coalesce(sum(r.amount), 0) as r_amount,
//		coalesce(sum(ss.amount), 0) as ss_amount,
//		coalesce(sum(fsu.amount), 0) as fsu_amount,
//		coalesce(sum(rsu.amount), 0) as rsu_amount,
//		coalesce(sum(ssu.amount), 0) as ssu_amount,
//		coalesce(sum(tsu.amount), 0) as tsu_amount,
//		coalesce(sum(os.amount), 0) as os_amount,
//		coalesce(sum(bp.amount), 0) as bp_amount
//	from regop_ro_payment_account ropa
//		cross join regop_period per
//
//		inner join regop_wallet btw on btw.id = ropa.bt_wallet_id
//		left join view_credit_loan_operations bt on bt.guid = btw.wallet_guid
//			and bt.operation_date >= per.cstart
//			and (per.cend is null or bt.operation_date <= per.cend)
//
//		inner join regop_wallet dtw on dtw.id = ropa.dt_wallet_id
//		left join view_credit_loan_operations dt on dt.guid = dtw.wallet_guid
//			and dt.operation_date >= per.cstart
//			and (per.cend is null or dt.operation_date <= per.cend)
//
//		inner join regop_wallet pw on pw.id = ropa.p_wallet_id
//		left join view_credit_loan_operations p on p.guid = pw.wallet_guid
//			and p.operation_date >= per.cstart
//			and (per.cend is null or p.operation_date <= per.cend)
//
//		inner join regop_wallet afw on afw.id = ropa.af_wallet_id
//		left join view_credit_loan_operations af on af.guid = afw.wallet_guid
//			and af.operation_date >= per.cstart
//			and (per.cend is null or af.operation_date <= per.cend)
//
//		inner join regop_wallet pwpw on pwpw.id = ropa.pwp_wallet_id
//		left join view_credit_loan_operations pwp on pwp.guid = pwpw.wallet_guid
//			and pwp.operation_date >= per.cstart
//			and (per.cend is null or pwp.operation_date <= per.cend)
//
//		inner join regop_wallet rw on rw.id = ropa.r_wallet_id
//		left join view_credit_loan_operations r on r.guid = rw.wallet_guid
//			and r.operation_date >= per.cstart
//			and (per.cend is null or r.operation_date <= per.cend)
//
//		inner join regop_wallet ssw on ssw.id = ropa.ss_wallet_id
//		left join view_credit_loan_operations ss on ss.guid = ssw.wallet_guid
//			and ss.operation_date >= per.cstart
//			and (per.cend is null or ss.operation_date <= per.cend)
//
//		inner join regop_wallet fsuw on fsuw.id = ropa.fsu_wallet_id
//		left join view_credit_loan_operations fsu on fsu.guid = fsuw.wallet_guid
//			and fsu.operation_date >= per.cstart
//			and (per.cend is null or fsu.operation_date <= per.cend)
//
//		inner join regop_wallet rsuw on rsuw.id = ropa.rsu_wallet_id
//		left join view_credit_loan_operations rsu on rsu.guid = rsuw.wallet_guid
//			and rsu.operation_date >= per.cstart
//			and (per.cend is null or rsu.operation_date <= per.cend)
//
//		inner join regop_wallet ssuw on ssuw.id = ropa.ssu_wallet_id
//		left join view_credit_loan_operations ssu on ssu.guid = ssuw.wallet_guid
//			and ssu.operation_date >= per.cstart
//			and (per.cend is null or ssu.operation_date <= per.cend)
//
//		inner join regop_wallet tsuw on tsuw.id = ropa.tsu_wallet_id
//		left join view_credit_loan_operations tsu on tsu.guid = tsuw.wallet_guid
//			and tsu.operation_date >= per.cstart
//			and (per.cend is null or tsu.operation_date <= per.cend)
//
//		inner join regop_wallet osw on osw.id = ropa.os_wallet_id
//		left join view_credit_loan_operations os on os.guid = osw.wallet_guid
//			and os.operation_date >= per.cstart
//			and (per.cend is null or os.operation_date <= per.cend)
//
//		inner join regop_wallet bpw on bpw.id = ropa.bp_wallet_id
//		left join view_credit_loan_operations bp on bp.guid = bpw.wallet_guid
//			and bp.operation_date >= per.cstart
//			and (per.cend is null or bp.operation_date <= per.cend)
//	group by ropa.ro_id, per.id
//) tt");

//                //кредитовые незаймовые суммы по домам
//                Database.ExecuteNonQuery(@"
//create materialized view view_robject_credit_sum as
//
//select
//	tt.ro_id,
//	tt.p_id,
//	tt.bt_amount,
//	tt.dt_amount,
//	tt.p_amount,
//	tt.af_amount,
//	tt.pwp_amount,
//	tt.r_amount,
//	tt.ss_amount,
//	tt.fsu_amount,
//	tt.rsu_amount,
//	tt.ssu_amount,
//	tt.tsu_amount,
//	tt.os_amount,
//	tt.bp_amount,
//	
//	tt.bt_amount + tt.dt_amount + tt.p_amount
//		+ tt.af_amount + tt.pwp_amount + tt.r_amount
//		+ tt.ss_amount + tt.fsu_amount + tt.rsu_amount
//		+ tt.ssu_amount	+ tt.tsu_amount	+ tt.os_amount
//		+ tt.bp_amount as amount
//from (
//	select
//		ropa.ro_id,
//		per.id as p_id,
//		coalesce(sum(bt.amount), 0) as bt_amount,
//		coalesce(sum(dt.amount), 0) as dt_amount,
//		coalesce(sum(p.amount), 0) as p_amount,
//		coalesce(sum(af.amount), 0) as af_amount,
//		coalesce(sum(pwp.amount), 0) as pwp_amount,
//		coalesce(sum(r.amount), 0) as r_amount,
//		coalesce(sum(ss.amount), 0) as ss_amount,
//		coalesce(sum(fsu.amount), 0) as fsu_amount,
//		coalesce(sum(rsu.amount), 0) as rsu_amount,
//		coalesce(sum(ssu.amount), 0) as ssu_amount,
//		coalesce(sum(tsu.amount), 0) as tsu_amount,
//		coalesce(sum(os.amount), 0) as os_amount,
//		coalesce(sum(bp.amount), 0) as bp_amount
//	from regop_ro_payment_account ropa
//		cross join regop_period per
//
//		inner join regop_wallet btw on btw.id = ropa.bt_wallet_id
//		left join view_credit_operations bt on bt.guid = btw.wallet_guid
//			and bt.operation_date >= per.cstart
//			and (per.cend is null or bt.operation_date <= per.cend)
//
//		inner join regop_wallet dtw on dtw.id = ropa.dt_wallet_id
//		left join view_credit_operations dt on dt.guid = dtw.wallet_guid
//			and dt.operation_date >= per.cstart
//			and (per.cend is null or dt.operation_date <= per.cend)
//
//		inner join regop_wallet pw on pw.id = ropa.p_wallet_id
//		left join view_credit_operations p on p.guid = pw.wallet_guid
//			and p.operation_date >= per.cstart
//			and (per.cend is null or p.operation_date <= per.cend)
//
//		inner join regop_wallet afw on afw.id = ropa.af_wallet_id
//		left join view_credit_operations af on af.guid = afw.wallet_guid
//			and af.operation_date >= per.cstart
//			and (per.cend is null or af.operation_date <= per.cend)
//
//		inner join regop_wallet pwpw on pwpw.id = ropa.pwp_wallet_id
//		left join view_credit_operations pwp on pwp.guid = pwpw.wallet_guid
//			and pwp.operation_date >= per.cstart
//			and (per.cend is null or pwp.operation_date <= per.cend)
//
//		inner join regop_wallet rw on rw.id = ropa.r_wallet_id
//		left join view_credit_operations r on r.guid = rw.wallet_guid
//			and r.operation_date >= per.cstart
//			and (per.cend is null or r.operation_date <= per.cend)
//
//		inner join regop_wallet ssw on ssw.id = ropa.ss_wallet_id
//		left join view_credit_operations ss on ss.guid = ssw.wallet_guid
//			and ss.operation_date >= per.cstart
//			and (per.cend is null or ss.operation_date <= per.cend)
//
//		inner join regop_wallet fsuw on fsuw.id = ropa.fsu_wallet_id
//		left join view_credit_operations fsu on fsu.guid = fsuw.wallet_guid
//			and fsu.operation_date >= per.cstart
//			and (per.cend is null or fsu.operation_date <= per.cend)
//
//		inner join regop_wallet rsuw on rsuw.id = ropa.rsu_wallet_id
//		left join view_credit_operations rsu on rsu.guid = rsuw.wallet_guid
//			and rsu.operation_date >= per.cstart
//			and (per.cend is null or rsu.operation_date <= per.cend)
//
//		inner join regop_wallet ssuw on ssuw.id = ropa.ssu_wallet_id
//		left join view_credit_operations ssu on ssu.guid = ssuw.wallet_guid
//			and ssu.operation_date >= per.cstart
//			and (per.cend is null or ssu.operation_date <= per.cend)
//
//		inner join regop_wallet tsuw on tsuw.id = ropa.tsu_wallet_id
//		left join view_credit_operations tsu on tsu.guid = tsuw.wallet_guid
//			and tsu.operation_date >= per.cstart
//			and (per.cend is null or tsu.operation_date <= per.cend)
//
//		inner join regop_wallet osw on osw.id = ropa.os_wallet_id
//		left join view_credit_operations os on os.guid = osw.wallet_guid
//			and os.operation_date >= per.cstart
//			and (per.cend is null or os.operation_date <= per.cend)
//
//		inner join regop_wallet bpw on bpw.id = ropa.bp_wallet_id
//		left join view_credit_operations bp on bp.guid = bpw.wallet_guid
//			and bp.operation_date >= per.cstart
//			and (per.cend is null or bp.operation_date <= per.cend)
//	group by ropa.ro_id, per.id
//) tt");

//                //дебетовые займовые суммы по домам
//                Database.ExecuteNonQuery(@"
//drop view if exists view_robject_debet_loan_sum;
//
//create materialized view view_robject_debet_loan_sum as
//
//select
//	tt.ro_id,
//	tt.p_id,
//	tt.bt_amount,
//	tt.dt_amount,
//	tt.p_amount,
//	tt.af_amount,
//	tt.pwp_amount,
//	tt.r_amount,
//	tt.ss_amount,
//	tt.fsu_amount,
//	tt.rsu_amount,
//	tt.ssu_amount,
//	tt.tsu_amount,
//	tt.os_amount,
//	tt.bp_amount,
//	
//	tt.bt_amount + tt.dt_amount + tt.p_amount
//		+ tt.af_amount + tt.pwp_amount + tt.r_amount
//		+ tt.ss_amount + tt.fsu_amount + tt.rsu_amount
//		+ tt.ssu_amount	+ tt.tsu_amount	+ tt.os_amount
//		+ tt.bp_amount as amount
//from (
//	select
//		ropa.ro_id,
//		per.id as p_id,
//		coalesce(sum(bt.amount), 0) as bt_amount,
//		coalesce(sum(dt.amount), 0) as dt_amount,
//		coalesce(sum(p.amount), 0) as p_amount,
//		coalesce(sum(af.amount), 0) as af_amount,
//		coalesce(sum(pwp.amount), 0) as pwp_amount,
//		coalesce(sum(r.amount), 0) as r_amount,
//		coalesce(sum(ss.amount), 0) as ss_amount,
//		coalesce(sum(fsu.amount), 0) as fsu_amount,
//		coalesce(sum(rsu.amount), 0) as rsu_amount,
//		coalesce(sum(ssu.amount), 0) as ssu_amount,
//		coalesce(sum(tsu.amount), 0) as tsu_amount,
//		coalesce(sum(os.amount), 0) as os_amount,
//		coalesce(sum(bp.amount), 0) as bp_amount
//	from regop_ro_payment_account ropa
//		cross join regop_period per
//
//		inner join regop_wallet btw on btw.id = ropa.bt_wallet_id
//		left join view_debet_loan_operations bt on bt.guid = btw.wallet_guid
//			and bt.operation_date >= per.cstart
//			and (per.cend is null or bt.operation_date <= per.cend)
//
//		inner join regop_wallet dtw on dtw.id = ropa.dt_wallet_id
//		left join view_debet_loan_operations dt on dt.guid = dtw.wallet_guid
//			and dt.operation_date >= per.cstart
//			and (per.cend is null or dt.operation_date <= per.cend)
//
//		inner join regop_wallet pw on pw.id = ropa.p_wallet_id
//		left join view_debet_loan_operations p on p.guid = pw.wallet_guid
//			and p.operation_date >= per.cstart
//			and (per.cend is null or p.operation_date <= per.cend)
//
//		inner join regop_wallet afw on afw.id = ropa.af_wallet_id
//		left join view_debet_loan_operations af on af.guid = afw.wallet_guid
//			and af.operation_date >= per.cstart
//			and (per.cend is null or af.operation_date <= per.cend)
//
//		inner join regop_wallet pwpw on pwpw.id = ropa.pwp_wallet_id
//		left join view_debet_loan_operations pwp on pwp.guid = pwpw.wallet_guid
//			and pwp.operation_date >= per.cstart
//			and (per.cend is null or pwp.operation_date <= per.cend)
//
//		inner join regop_wallet rw on rw.id = ropa.r_wallet_id
//		left join view_debet_loan_operations r on r.guid = rw.wallet_guid
//			and r.operation_date >= per.cstart
//			and (per.cend is null or r.operation_date <= per.cend)
//
//		inner join regop_wallet ssw on ssw.id = ropa.ss_wallet_id
//		left join view_debet_loan_operations ss on ss.guid = ssw.wallet_guid
//			and ss.operation_date >= per.cstart
//			and (per.cend is null or ss.operation_date <= per.cend)
//
//		inner join regop_wallet fsuw on fsuw.id = ropa.fsu_wallet_id
//		left join view_debet_loan_operations fsu on fsu.guid = fsuw.wallet_guid
//			and fsu.operation_date >= per.cstart
//			and (per.cend is null or fsu.operation_date <= per.cend)
//
//		inner join regop_wallet rsuw on rsuw.id = ropa.rsu_wallet_id
//		left join view_debet_loan_operations rsu on rsu.guid = rsuw.wallet_guid
//			and rsu.operation_date >= per.cstart
//			and (per.cend is null or rsu.operation_date <= per.cend)
//
//		inner join regop_wallet ssuw on ssuw.id = ropa.ssu_wallet_id
//		left join view_debet_loan_operations ssu on ssu.guid = ssuw.wallet_guid
//			and ssu.operation_date >= per.cstart
//			and (per.cend is null or ssu.operation_date <= per.cend)
//
//		inner join regop_wallet tsuw on tsuw.id = ropa.tsu_wallet_id
//		left join view_debet_loan_operations tsu on tsu.guid = tsuw.wallet_guid
//			and tsu.operation_date >= per.cstart
//			and (per.cend is null or tsu.operation_date <= per.cend)
//
//		inner join regop_wallet osw on osw.id = ropa.os_wallet_id
//		left join view_debet_loan_operations os on os.guid = osw.wallet_guid
//			and os.operation_date >= per.cstart
//			and (per.cend is null or os.operation_date <= per.cend)
//
//		inner join regop_wallet bpw on bpw.id = ropa.bp_wallet_id
//		left join view_debet_loan_operations bp on bp.guid = bpw.wallet_guid
//			and bp.operation_date >= per.cstart
//			and (per.cend is null or bp.operation_date <= per.cend)
//	group by ropa.ro_id, per.id
//) tt");

//                //дебетовые незаймовые суммы по домам
//                Database.ExecuteNonQuery(@"
//create materialized view view_robject_debet_sum as
//
//select
//	tt.ro_id,
//	tt.p_id,
//	tt.bt_amount,
//	tt.dt_amount,
//	tt.p_amount,
//	tt.af_amount,
//	tt.pwp_amount,
//	tt.r_amount,
//	tt.ss_amount,
//	tt.fsu_amount,
//	tt.rsu_amount,
//	tt.ssu_amount,
//	tt.tsu_amount,
//	tt.os_amount,
//	tt.bp_amount,
//	
//	tt.bt_amount + tt.dt_amount + tt.p_amount
//		+ tt.af_amount + tt.pwp_amount + tt.r_amount
//		+ tt.ss_amount + tt.fsu_amount + tt.rsu_amount
//		+ tt.ssu_amount	+ tt.tsu_amount	+ tt.os_amount
//		+ tt.bp_amount as amount
//from (
//	select
//		ropa.ro_id,
//		per.id as p_id,
//		coalesce(sum(bt.amount), 0) as bt_amount,
//		coalesce(sum(dt.amount), 0) as dt_amount,
//		coalesce(sum(p.amount), 0) as p_amount,
//		coalesce(sum(af.amount), 0) as af_amount,
//		coalesce(sum(pwp.amount), 0) as pwp_amount,
//		coalesce(sum(r.amount), 0) as r_amount,
//		coalesce(sum(ss.amount), 0) as ss_amount,
//		coalesce(sum(fsu.amount), 0) as fsu_amount,
//		coalesce(sum(rsu.amount), 0) as rsu_amount,
//		coalesce(sum(ssu.amount), 0) as ssu_amount,
//		coalesce(sum(tsu.amount), 0) as tsu_amount,
//		coalesce(sum(os.amount), 0) as os_amount,
//		coalesce(sum(bp.amount), 0) as bp_amount
//	from regop_ro_payment_account ropa
//		cross join regop_period per
//
//		inner join regop_wallet btw on btw.id = ropa.bt_wallet_id
//		left join view_debet_operations bt on bt.guid = btw.wallet_guid
//			and bt.operation_date >= per.cstart
//			and (per.cend is null or bt.operation_date <= per.cend)
//
//		inner join regop_wallet dtw on dtw.id = ropa.dt_wallet_id
//		left join view_debet_operations dt on dt.guid = dtw.wallet_guid
//			and dt.operation_date >= per.cstart
//			and (per.cend is null or dt.operation_date <= per.cend)
//
//		inner join regop_wallet pw on pw.id = ropa.p_wallet_id
//		left join view_debet_operations p on p.guid = pw.wallet_guid
//			and p.operation_date >= per.cstart
//			and (per.cend is null or p.operation_date <= per.cend)
//
//		inner join regop_wallet afw on afw.id = ropa.af_wallet_id
//		left join view_debet_operations af on af.guid = afw.wallet_guid
//			and af.operation_date >= per.cstart
//			and (per.cend is null or af.operation_date <= per.cend)
//
//		inner join regop_wallet pwpw on pwpw.id = ropa.pwp_wallet_id
//		left join view_debet_operations pwp on pwp.guid = pwpw.wallet_guid
//			and pwp.operation_date >= per.cstart
//			and (per.cend is null or pwp.operation_date <= per.cend)
//
//		inner join regop_wallet rw on rw.id = ropa.r_wallet_id
//		left join view_debet_operations r on r.guid = rw.wallet_guid
//			and r.operation_date >= per.cstart
//			and (per.cend is null or r.operation_date <= per.cend)
//
//		inner join regop_wallet ssw on ssw.id = ropa.ss_wallet_id
//		left join view_debet_operations ss on ss.guid = ssw.wallet_guid
//			and ss.operation_date >= per.cstart
//			and (per.cend is null or ss.operation_date <= per.cend)
//
//		inner join regop_wallet fsuw on fsuw.id = ropa.fsu_wallet_id
//		left join view_debet_operations fsu on fsu.guid = fsuw.wallet_guid
//			and fsu.operation_date >= per.cstart
//			and (per.cend is null or fsu.operation_date <= per.cend)
//
//		inner join regop_wallet rsuw on rsuw.id = ropa.rsu_wallet_id
//		left join view_debet_operations rsu on rsu.guid = rsuw.wallet_guid
//			and rsu.operation_date >= per.cstart
//			and (per.cend is null or rsu.operation_date <= per.cend)
//
//		inner join regop_wallet ssuw on ssuw.id = ropa.ssu_wallet_id
//		left join view_debet_operations ssu on ssu.guid = ssuw.wallet_guid
//			and ssu.operation_date >= per.cstart
//			and (per.cend is null or ssu.operation_date <= per.cend)
//
//		inner join regop_wallet tsuw on tsuw.id = ropa.tsu_wallet_id
//		left join view_debet_operations tsu on tsu.guid = tsuw.wallet_guid
//			and tsu.operation_date >= per.cstart
//			and (per.cend is null or tsu.operation_date <= per.cend)
//
//		inner join regop_wallet osw on osw.id = ropa.os_wallet_id
//		left join view_debet_operations os on os.guid = osw.wallet_guid
//			and os.operation_date >= per.cstart
//			and (per.cend is null or os.operation_date <= per.cend)
//
//		inner join regop_wallet bpw on bpw.id = ropa.bp_wallet_id
//		left join view_debet_operations bp on bp.guid = bpw.wallet_guid
//			and bp.operation_date >= per.cstart
//			and (per.cend is null or bp.operation_date <= per.cend)
//	group by ropa.ro_id, per.id
//) tt");
//            }
//            catch (Exception)
//            {
//                //на pg версии < 9.3 упадет, поэтому так
//            }
        }

        public override void Down()
        {
            //Database.ExecuteNonQuery(@"drop materialized view if exists view_robject_credit_loan_sum");
            //Database.ExecuteNonQuery(@"drop materialized view if exists view_robject_credit_sum");
            //Database.ExecuteNonQuery(@"drop materialized view if exists view_robject_debet_loan_sum");
            //Database.ExecuteNonQuery(@"drop materialized view if exists view_robject_debet_sum");

            //Database.ExecuteNonQuery(@"drop view if exists view_debet_operations");
            //Database.ExecuteNonQuery(@"drop view if exists view_credit_operations");
            //Database.ExecuteNonQuery(@"drop view if exists view_debet_loan_operations");
            //Database.ExecuteNonQuery(@"drop view if exists view_credit_loan_operations");
        }
    }
}
