namespace Bars.GkhDi.Migrations._2019.Version_2019070500
{
    using Bars.B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019070500")]
    [MigrationDependsOn(typeof(_2019.Version_2019030400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
CREATE SCHEMA if not exists service
  AUTHORIZATION bars;

delete  
from DI_DISINFO_RELATION r
where exists (select 1 from (select min(d.id), disinfo_ro_id,disinfo_id
from DI_DISINFO_RELATION d
join DI_DISINFO_REALOBJ dr on dr.id=d.disinfo_ro_id
group by disinfo_ro_id,disinfo_id
having count(*)>1) t where t.min<>r.id and r.disinfo_ro_id=t.disinfo_ro_id and t.disinfo_id=r.disinfo_id);

drop index unq_period_di_ro;

CREATE UNIQUE INDEX unq_period_di_ro
  ON public.di_disinfo_realobj
  USING btree
  (period_di_id, reality_obj_id, man_org_id);

drop table if exists tmp1;
create temp table tmp1 as
select dro.id disinfo_ro_id, r.id rel_id, dro.period_di_id, ro.id ro_id, d.manag_org_id, d.id disinfo_id, min(moc.start_date)start_date
from gkh_morg_contract_realobj mcro 
	join gkh_morg_contract moc on moc.id = mcro.man_org_contract_id 
	join gkh_reality_object ro on ro.id=reality_obj_id 
	JOIN gkh_managing_organization mo ON mo.id = moc.manag_org_id 
	join di_disinfo d on d.manag_org_id=mo.id
	join di_dict_period p on p.id= d.period_di_id and p.date_start<=coalesce(end_date, '01.01.3001') and p.date_end>=coalesce(moc.start_date, '01.01.0001')
	join di_disinfo_realobj dro on dro.reality_obj_id=ro.id  and p.id=dro.period_di_id 
	left join di_disinfo_relation r  on dro.id=r.disinfo_ro_id and d.id=r.disinfo_id 
	/*where type_house not in (10,20) 
	and condition_house  in (30,10,20) and not RESIDENTS_EVICTED */
	group by dro.id , r.id , dro.period_di_id, ro.id , d.manag_org_id, d.id 
	order by 1, 3;

drop table if exists  a;
create temp table a as
	select disinfo_ro_id ,period_di_id, count(*)  from tmp1 
	group by disinfo_ro_id, period_di_id
	having count(*)>1;

drop table if exists service.tmp_relation;
create table service.tmp_relation as
select  r.rel_id id, r.disinfo_id, d.period_di_id, ro.id  disinfo_ro_id, rr.disinfo_ro_id old_disinfo_ro_id , r.manag_org_id , reality_obj_id,  
		row_number () over (partition by ro.period_di_id, ro.id order by r.start_date ) n , null::int new_disinfo_ro_id, start_date
	from tmp1 r 
	left join DI_DISINFO_RELATION rr on rr.id=r.rel_id
	join DI_DISINFO_REALOBJ ro on ro.id=r.disinfo_ro_id  
	join di_disinfo d on d.id=r.disinfo_id and ro.period_di_id=d.period_di_id 
where exists (select 1 from a where a.disinfo_ro_id=ro.id and a.period_di_id = ro.period_di_id);

insert into DI_DISINFO_RELATION (object_version, object_create_date, object_edit_date,disinfo_id, disinfo_ro_id, import_entity_id)
select 18994, 'now'::timestamp(0),'now'::timestamp(0),disinfo_id, disinfo_ro_id, n
 from service.tmp_relation where id is null;

update service.tmp_relation r set id=d.id from 
DI_DISINFO_RELATION d where object_version=18994 and d.disinfo_id=r.disinfo_id and d.disinfo_ro_id=r.disinfo_ro_id and import_entity_id=n and r.id is null;

alter table service.tmp_relation add column if not exists create_new boolean default false;

update service.tmp_relation t set create_new= true where 
	not exists (select 1 from  service.tmp_relation tt where tt.disinfo_ro_id=t.disinfo_ro_id and tt.old_disinfo_ro_id is null) and n<>1;
update service.tmp_relation t set create_new= true where 
	exists (select 1 from  service.tmp_relation tt where tt.disinfo_ro_id=t.disinfo_ro_id and tt.old_disinfo_ro_id is null)
	and exists (select 1 from  service.tmp_relation tt where tt.disinfo_ro_id=t.disinfo_ro_id and tt.old_disinfo_ro_id is not null) and old_disinfo_ro_id is null; 
update service.tmp_relation t set create_new= true where 
	exists (select 1 from  service.tmp_relation tt where tt.disinfo_ro_id=t.disinfo_ro_id and tt.old_disinfo_ro_id is null)
	and exists (select 1 from (select min(n),disinfo_ro_id  from service.tmp_relation tt where  tt.old_disinfo_ro_id is not null group by disinfo_ro_id) tt where tt.disinfo_ro_id=t.disinfo_ro_id and min<>t.n) 
	and old_disinfo_ro_id is not null ;
update service.tmp_relation t set create_new= true where 
	exists (select 1 from  service.tmp_relation tt where tt.disinfo_ro_id=t.disinfo_ro_id and tt.old_disinfo_ro_id is null)
	and not exists (select 1 from  service.tmp_relation tt where tt.disinfo_ro_id=t.disinfo_ro_id and tt.old_disinfo_ro_id is not null) and n<>1 ;

INSERT INTO di_disinfo_realobj(
            object_version, object_create_date, object_edit_date, reality_obj_id, 
            period_di_id, non_resident_place, place_general_use, claim_compensation, 
            claim_non_service, claim_non_delivery, work_repair, work_landscaping, 
            subsidies, credit, finance_leasing, finance_energy, occupant_contrib, 
            other_source, reduction_payment, external_id, execution_work, 
            execution_obligation, descr_catrep_serv, descr_catrep_tarif, 
            file_execution_work, file_ecxecution_oblig, file_serv_cat_report, 
            file_tarif_cat_report, advance_payments, carryover_funds, debt, 
            cfmar_maintance, cfmar_repairs, rc_fowners, rc_fotargeted, rc_grant, 
            rc_fcommonprop, rc_fother, cb_advance, cb_carryover, cb_debt, 
            all_cash_balance, all_received_cash, receive_pretension_cnt, 
            aprove_pretension_cnt, no_aprove_pretension_cnt, pretension_recalc_sum, 
            sent_pretension_cnt, sent_petition_cnt, receive_sum_by_clw, cfmar_managment, 
            cfmar_all, com_srv_start_advance_pay, com_srv_start_carryov_fnd, 
            com_srv_start_debt, com_srv_end_advance_pay, com_srv_end_carryov_fnd, 
            com_srv_end_debt, com_srv_rcv_preten_cnt, com_srv_aprove_preten_cnt, 
            com_srv_no_apr_preten_cnt, com_srv_preten_recalc_sum, import_entity_id, man_org_id)
select 18994, 'now'::timestamp(0),'now'::timestamp(0),r.reality_obj_id, 
            r.period_di_id, non_resident_place, place_general_use, claim_compensation, 
            claim_non_service, claim_non_delivery, work_repair, work_landscaping, 
            subsidies, credit, finance_leasing, finance_energy, occupant_contrib, 
            other_source, reduction_payment, external_id, execution_work, 
            execution_obligation, descr_catrep_serv, descr_catrep_tarif, 
            file_execution_work, file_ecxecution_oblig, file_serv_cat_report, 
            file_tarif_cat_report, advance_payments, carryover_funds, debt, 
            cfmar_maintance, cfmar_repairs, rc_fowners, rc_fotargeted, rc_grant, 
            rc_fcommonprop, rc_fother, cb_advance, cb_carryover, cb_debt, 
            all_cash_balance, all_received_cash, receive_pretension_cnt, 
            aprove_pretension_cnt, no_aprove_pretension_cnt, pretension_recalc_sum, 
            sent_pretension_cnt, sent_petition_cnt, receive_sum_by_clw, cfmar_managment, 
            cfmar_all, com_srv_start_advance_pay, com_srv_start_carryov_fnd, 
            com_srv_start_debt, com_srv_end_advance_pay, com_srv_end_carryov_fnd, 
            com_srv_end_debt, com_srv_rcv_preten_cnt, com_srv_aprove_preten_cnt, 
            com_srv_no_apr_preten_cnt, com_srv_preten_recalc_sum,   t.id, t.manag_org_id
from service.tmp_relation t
join DI_DISINFO_REALOBJ r on r.id=disinfo_ro_id
where create_new;

update service.tmp_relation t set new_disinfo_ro_id = r.id from
	DI_DISINFO_REALOBJ r where r.import_entity_id=t.id;
update service.tmp_relation t set new_disinfo_ro_id = disinfo_ro_id where 
	new_disinfo_ro_id is null and create_new=false;

update DI_DISINFO_RELATION r set disinfo_ro_id = new_disinfo_ro_id from 
	service.tmp_relation t where t.id=r.id and r.disinfo_ro_id <> new_disinfo_ro_id ; 

update DI_DISINFO_REALOBJ ro set man_org_id=manag_org_id
	from  
(select  distinct r.*, d.manag_org_id, dro.period_di_id
	from DI_DISINFO_REALOBJ dro 
	join DI_DISINFO_RELATION r on r.disinfo_ro_id=dro.id 
	join DI_DISINFO d on r.disinfo_id=d.id
	join di_dict_period p on p.id= d.period_di_id
	join gkh_reality_object ro on ro.id=dro.reality_obj_id
	join gkh_morg_contract_realobj mcro on ro.id=mcro.reality_obj_id  
	join gkh_morg_contract moc on moc.id = mcro.man_org_contract_id  and p.date_start<=coalesce(end_date, '01.01.3001') and p.date_end>=coalesce(moc.start_date, '01.01.0001') and moc.manag_org_id=d.manag_org_id) r
	where r.disinfo_ro_id=ro.id and ro.man_org_id is null;


delete from DI_DISINFO_RELATION where disinfo_ro_id in (select id from DI_DISINFO_REALOBJ where man_org_id is null);

create index on service.tmp_relation(disinfo_ro_id);

insert into di_disinfo_com_facils (object_version, object_create_date, object_edit_date,  disinfo_ro_id,
  kind_common_facilities,  num,  date_from,  lessee,  type_contract, date_start , date_end ,
  cost_contract,  external_id,  appointment_common_facilities, area_of_common_facilities,
  contract_number, contract_date, cost_by_contract_in_month,   lessee_type, surname,
  name, gender, birth_date, birth_place, snils, ogrn, contract_subject, protocol_file_id, contract_file_id,  
  patronymic, facils_comment, signing_contract_date, inn, day_month_period_in , day_month_period_out ,
  is_last_day_month_period_in, is_last_day_month_period_out, is_next_month_period_in,
  is_next_month_period_out, import_entity_id)
select 18994, object_create_date, object_edit_date,  new_disinfo_ro_id,
  kind_common_facilities,  num,  date_from,  lessee,  type_contract, date_start , date_end ,
  cost_contract,  external_id,  appointment_common_facilities, area_of_common_facilities,
  contract_number, contract_date, cost_by_contract_in_month,   lessee_type, surname,
  name, gender, birth_date, birth_place, snils, ogrn, contract_subject, null, null,   
  patronymic, facils_comment, signing_contract_date, inn, day_month_period_in , day_month_period_out ,
  is_last_day_month_period_in, is_last_day_month_period_out, is_next_month_period_in,
  is_next_month_period_out, import_entity_id
from di_disinfo_com_facils f 
join service.tmp_relation t on t.disinfo_ro_id=f.disinfo_ro_id and create_new;

insert into di_disinfo_doc_ro ( object_version, object_create_date, object_edit_date, disinfo_ro_id, 
	file_act_state_id, file_catrepair_id, file_plan_repair_id, descr_act_state, external_id, 
	has_gen_meet_owners, import_entity_id)
select 18994, object_create_date, object_edit_date, new_disinfo_ro_id, 
	null, null, null, descr_act_state, external_id, 
	has_gen_meet_owners, import_entity_id
from di_disinfo_doc_ro f
	join service.tmp_relation t on t.disinfo_ro_id=f.disinfo_ro_id and create_new;

insert into di_disinfo_fincommun_ro ( object_version, object_create_date, object_edit_date, disinfo_ro_id, 
	type_service_di, paid_owner, debt_owner, paid_by_indicator, paid_by_account, external_id, import_entity_id)
select 18994, object_create_date, object_edit_date, new_disinfo_ro_id, 
	type_service_di, paid_owner, debt_owner, paid_by_indicator, paid_by_account, external_id, import_entity_id
	from di_disinfo_fincommun_ro f
	join service.tmp_relation t on t.disinfo_ro_id=f.disinfo_ro_id and create_new;

insert into di_empty_fields_realobj_log( object_version, object_create_date, object_edit_date, name, path_id, di_reality_obj_id, import_entity_id)
select 18994, object_create_date, object_edit_date, name, path_id, new_disinfo_ro_id, import_entity_id
	from di_empty_fields_realobj_log f
	join service.tmp_relation t on t.disinfo_ro_id=f.di_reality_obj_id and create_new;

insert into di_other_service ( object_version, object_create_date, object_edit_date, disinfo_ro_id, 
	name, unit_measure, tariff, provider, external_id, code, import_entity_id)
select 18994, object_create_date, object_edit_date, new_disinfo_ro_id, 
	name, unit_measure, tariff, provider, external_id, code, import_entity_id
from di_other_service f
	join service.tmp_relation t on t.disinfo_ro_id=f.disinfo_ro_id and create_new;

with perc_calc as (
insert into di_arch_perc_calc ( object_version, object_create_date, object_edit_date, code, actual_version, calc_date, percent, position_cnt, complete_posit_cnt, type_entity_perc_calc)
select new_disinfo_ro_id, object_create_date, object_edit_date, code, actual_version, calc_date, percent, position_cnt, complete_posit_cnt, type_entity_perc_calc
	from di_arch_perc_calc p 
	join di_arch_perc_real_obj pro on pro.id=p.id
	join service.tmp_relation t on t.disinfo_ro_id=pro.real_obj_id and create_new
returning id, object_version)
insert into di_arch_perc_real_obj (id, real_obj_id)
select id, object_version from perc_calc;

update di_arch_perc_calc p set object_version=18994 where 
	exists (select 1 from  di_arch_perc_real_obj ro join service.tmp_relation t on t.new_disinfo_ro_id=ro.real_obj_id and create_new where p.id=ro.id );

insert into di_disinfo_ro_nonresplace( object_version, object_create_date, object_edit_date, disinfo_ro_id, 
	type_contragent, area, contragent_name, doc_num_apartment, doc_date_apartment, doc_name_apartment, 
	doc_num_communal, doc_date_communal, doc_name_communal, date_start, date_end, external_id, import_entity_id)
select 18994, object_create_date, object_edit_date, new_disinfo_ro_id,
	type_contragent, area, contragent_name, doc_num_apartment, doc_date_apartment, doc_name_apartment, 
	doc_num_communal, doc_date_communal, doc_name_communal, date_start, date_end, external_id, f.id
from di_disinfo_ro_nonresplace f 
join service.tmp_relation t on t.disinfo_ro_id=f.disinfo_ro_id and create_new;

insert into di_disinfo_rononresp_metr(object_version, object_create_date, object_edit_date, di_nonresplace_id, 
	metering_device_id, external_id, import_entity_id)
select 18994, m.object_create_date, m.object_edit_date,t.id,
	m.metering_device_id, m.external_id, m.import_entity_id
from di_disinfo_ro_nonresplace t
	join service.tmp_relation r on r.new_disinfo_ro_id=t.disinfo_ro_id and create_new
	join di_disinfo_ro_nonresplace t_old on t_old.id=t.import_entity_id
	join di_disinfo_rononresp_metr m on m.di_nonresplace_id=t_old.id;


insert into di_base_service ( object_version, object_create_date, object_edit_date, disinfo_ro_id, 
	template_service_id, provider_id, unit_measure_id, profit, external_id, tariff, tariff_set_for, 
	date_start_tariff, schedule_prevent_job, import_entity_id)
select 18994, object_create_date, object_edit_date, new_disinfo_ro_id,
	template_service_id, provider_id, unit_measure_id, profit, external_id, tariff, tariff_set_for, 
	date_start_tariff, schedule_prevent_job, s.id
from di_base_service s 
join service.tmp_relation t on t.disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_addition_service ( id, periodicity_id, document, document_number, document_from, date_start, date_end, total)
select s.id, a.periodicity_id, a.document, a.document_number, a.document_from, a.date_start, a.date_end, a.total
from di_addition_service a 
join di_base_service s on a.id=import_entity_id
join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_cap_rep_service (id, type_of_provision_service, regional_fund)
select s.id, type_of_provision_service, regional_fund
	from di_cap_rep_service a
	join di_base_service s on a.id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_caprepair_work (object_version, object_create_date, object_edit_date, base_service_id, 
	work_id, plan_volume, plan_cost, fact_volume, fact_cost, external_id, import_entity_id)
select 18994 ,  a.object_create_date, a.object_edit_date, s.id, 
	a.work_id, a.plan_volume, a.plan_cost, a.fact_volume, a.fact_cost, a.external_id, a.import_entity_id
from di_caprepair_work a 
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_communal_service (id, volume_purchased_resources, price_purchased_resources, kind_electricity_supply, 
	type_of_provision_service, unit_measure_liv_house_id, additional_info_liv_house, cons_norm_housing, 
	unit_measure_housing_id, additional_info_housing, cons_norm_liv_house)
select s.id, a.volume_purchased_resources, a.price_purchased_resources, a.kind_electricity_supply, 
	a.type_of_provision_service, a.unit_measure_liv_house_id, a.additional_info_liv_house, a.cons_norm_housing, 
	a.unit_measure_housing_id, a.additional_info_housing, a.cons_norm_liv_house
from di_communal_service a
join  di_base_service s on a.id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_consumption_norms_npa (object_version, object_create_date, object_edit_date, base_service_id, 
	cons_norm_npa_date, cons_norm_npa_number, cons_norm_npa_acceptor, external_id, import_entity_id)
select 18994, a.object_create_date, a.object_edit_date,s.id, 
	a.cons_norm_npa_date, a.cons_norm_npa_number, a.cons_norm_npa_acceptor, a.external_id, a.import_entity_id
from di_consumption_norms_npa a
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_control_service(id)
select s.id from di_control_service a
join di_base_service s on a.id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_cost_item (object_version, object_create_date, object_edit_date, base_service_id, 
	name, cost, count, sum, external_id, import_entity_id)
select 18994,  a.object_create_date, a.object_edit_date, s.id, 
	a.name, a.cost, a.count, a.sum, a.external_id, a.import_entity_id
from di_cost_item a
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_housing_service (id, periodicity_id, protocol, equipment, protocol_number, protocol_from, type_of_provision_service)
select s.id, a.periodicity_id, null, equipment, protocol_number, protocol_from, type_of_provision_service
from di_housing_service a
join di_base_service s on a.id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_repair_service(id, type_of_provision_service, sum_work_to, sum_fact, progress_info, reject_cause, date_start, date_end)
select s.id, a.type_of_provision_service, a.sum_work_to, a.sum_fact, a.progress_info, a.reject_cause, a.date_start, a.date_end
from di_repair_service a
join di_base_service s on a.id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_repair_work_detail ( object_version, object_create_date, object_edit_date, base_service_id, 
	work_ppr_id, external_id, import_entity_id)
select 18994, a.object_create_date, a.object_edit_date, s.id, 
	a.work_ppr_id, a.external_id, a.import_entity_id
from di_repair_work_detail a 
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_repair_work_tech (object_version, object_create_date, object_edit_date, base_service_id, 
	work_to_id, external_id, import_entity_id)
select 18994, a.object_create_date, a.object_edit_date, s.id, 
	a.work_to_id, a.external_id, a.import_entity_id
from di_repair_work_tech a
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_service_provider( object_version, object_create_date, object_edit_date, base_service_id, 
	provider_id, date_start_contract, description, external_id, is_active, number_contract, import_entity_id)
select 18994,  a.object_create_date, a.object_edit_date, s.id,
	a.provider_id, a.date_start_contract, a.description, a.external_id, a.is_active, a.number_contract, a.import_entity_id
from di_service_provider a
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_tariff_fconsumers ( object_version, object_create_date, object_edit_date, base_service_id, 
	date_start, tariff_is_set_for, cost, organization_set_tariff, cost_night, type_organ_set_tariff, external_id, date_end, import_entity_id)
select 18994, a.object_create_date, a.object_edit_date, s.id, 
	a.date_start, a.tariff_is_set_for, a.cost, a.organization_set_tariff, a.cost_night, a.type_organ_set_tariff, a.external_id, a.date_end, a.import_entity_id
from di_tariff_fconsumers a
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_tariff_frso (object_version, object_create_date, object_edit_date, base_service_id, 
	date_start, number_normative_legal_act, date_normative_legal_act, organization_set_tariff, cost, cost_night, external_id, date_end, import_entity_id)
select 18994, a.object_create_date, a.object_edit_date, s.id, 
	a.date_start, a.number_normative_legal_act, a.date_normative_legal_act, a.organization_set_tariff, a.cost, a.cost_night, a.external_id, a.date_end, a.import_entity_id
from di_tariff_frso a
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_disinfo_ro_pay_commun (object_version, object_create_date, object_edit_date, disinfo_ro_id, base_service_id, 
	counter_value_start, counter_value_end, accrual, payed, debt, external_id, total_consump, accrual_by_prov, 
	payed_to_prov, debt_to_prov, received_penalty, import_entity_id) 
select 18994,  a.object_create_date, a.object_edit_date, new_disinfo_ro_id, s.id, 
	a.counter_value_start, a.counter_value_end, a.accrual, a.payed, a.debt, a.external_id, a.total_consump, a.accrual_by_prov, 
	a.payed_to_prov, a.debt_to_prov, a.received_penalty, a.import_entity_id 
from di_disinfo_ro_pay_commun a
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new and a.disinfo_ro_id=t.disinfo_ro_id;


insert into di_disinfo_ro_pay_housing (object_version, object_create_date, object_edit_date, disinfo_ro_id, base_service_id, 
	counter_value_start, counter_value_end, general_accraul, collection, external_id, import_entity_id)
select 18994, a.object_create_date, a.object_edit_date, new_disinfo_ro_id, s.id, 
	a.counter_value_start, a.counter_value_end, a.general_accraul, a.collection, a.external_id, a.import_entity_id
from di_disinfo_ro_pay_housing a
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new and a.disinfo_ro_id=t.disinfo_ro_id;

insert into di_disinfo_ro_reduct_pay (object_version, object_create_date, object_edit_date, disinfo_ro_id, base_service_id, 
	reason_reduction, recalc_sum, description, order_date, order_num, external_id, import_entity_id)
select 18994,  a.object_create_date, a.object_edit_date, new_disinfo_ro_id, s.id, 
	a.reason_reduction, a.recalc_sum, a.description, a.order_date, a.order_num, a.external_id, a.import_entity_id
from di_disinfo_ro_reduct_pay a
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new and a.disinfo_ro_id=t.disinfo_ro_id;

insert into di_disinfo_ro_reduct_exp(object_version, object_create_date, object_edit_date, disinfo_ro_id, base_service_id, external_id, import_entity_id)
select 18994,  a.object_create_date, a.object_edit_date, new_disinfo_ro_id, s.id, a.external_id, a.id
from di_disinfo_ro_reduct_exp a
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new and a.disinfo_ro_id=t.disinfo_ro_id;

insert into di_disinfo_ro_redexp_work (object_version, object_create_date, object_edit_date, disinfo_ro_redexp_id, 
	name, date_complete, plan_reduct_expense, fact_reduct_expense, reason_rejection, external_id, import_entity_id)
select 18994, a.object_create_date, a.object_edit_date, w.id,
	a.name, a.date_complete, a.plan_reduct_expense, a.fact_reduct_expense, a.reason_rejection, a.external_id, a.id
from di_disinfo_ro_redexp_work a
join di_disinfo_ro_reduct_exp w on w.import_entity_id=disinfo_ro_redexp_id
join di_base_service s on w.base_service_id=s.id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_tat_plan_red_meas_name (object_version, object_create_date, object_edit_date, measure_red_costs_id, plan_red_expwork_id)
select 18994,  a.object_create_date, a.object_edit_date, a.measure_red_costs_id, w.id
from di_tat_plan_red_meas_name a 
join di_disinfo_ro_redexp_work w on w.import_entity_id=a.plan_red_expwork_id and w.object_version=18994;

with perc_calc as (
insert into di_perc_calc ( object_version, object_create_date, object_edit_date, code, actual_version, calc_date, percent, position_cnt, complete_posit_cnt, type_entity_perc_calc)
select new_disinfo_ro_id, object_create_date, object_edit_date, code, actual_version, calc_date, percent, position_cnt, complete_posit_cnt, type_entity_perc_calc
	from di_perc_calc p 
	join di_perc_real_obj pro on pro.id=p.id
	join service.tmp_relation t on t.disinfo_ro_id=pro.real_obj_id and create_new
returning id, object_version)
insert into di_perc_real_obj (id, real_obj_id)
select id, object_version from perc_calc;
	update di_perc_calc p set object_version=18994 where 
	exists (select 1 from  di_perc_real_obj ro join service.tmp_relation t on t.new_disinfo_ro_id=ro.real_obj_id and create_new where p.id=ro.id );

with perc_calc as (
insert into di_perc_calc ( object_version, object_create_date, object_edit_date, code, actual_version, calc_date, percent, position_cnt, complete_posit_cnt, type_entity_perc_calc)
select s.id, p.object_create_date, p.object_edit_date, p.code, p.actual_version, p.calc_date, p.percent, p.position_cnt, p.complete_posit_cnt, p.type_entity_perc_calc
	from di_perc_calc p 
	join di_perc_service pro on pro.id=p.id
	join di_base_service s on s.import_entity_id=pro.service_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new
returning id, object_version)
insert into di_perc_service (id, service_id)
select id, object_version from perc_calc;
	update di_perc_calc p set object_version=18994 where 
	exists (select 1 from  di_perc_service ro join di_base_service s on s.id=service_id join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new where p.id=ro.id );

insert into di_disinfo_ro_serv_repair (object_version, object_create_date, object_edit_date, disinfo_ro_id, 
	base_service_id, external_id, import_entity_id) 
select 18994,  a.object_create_date, a.object_edit_date, new_disinfo_ro_id,
	s.id, a.external_id, a.id
from di_disinfo_ro_serv_repair a
join di_base_service s on a.base_service_id=s.import_entity_id
	join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new and a.disinfo_ro_id=t.disinfo_ro_id;

insert into di_repair_work_list (object_version, object_create_date, object_edit_date, base_service_id, 
	group_work_ppr_id, plan_volume, plan_cost, fact_volume, fact_cost, date_start, date_end, external_id, import_entity_id)
select 18994, a.object_create_date, a.object_edit_date, s.id, 
	a.group_work_ppr_id, a.plan_volume, a.plan_cost, a.fact_volume, 
	a.fact_cost, a.date_start, a.date_end, a.external_id, a.id
from di_repair_work_list a
 join di_base_service s on a.base_service_id=s.import_entity_id
 join service.tmp_relation t on t.new_disinfo_ro_id=s.disinfo_ro_id and create_new;

insert into di_disinfo_ro_srvrep_work(object_version, object_create_date, object_edit_date, disinfo_ro_srvrep_id, repair_work_id, 
	periodicity_id, data_complete, date_complete, cost, date_start, date_end, fact_cost, reason_rejection, external_id, import_entity_id)
select 18994, a.object_create_date, a.object_edit_date, sr.id, w.id, 
	a.periodicity_id, a.data_complete, a.date_complete, a.cost, a.date_start, a.date_end, a.fact_cost, a.reason_rejection, a.external_id, a.import_entity_id 
from di_disinfo_ro_srvrep_work a
join di_repair_work_list w on w.import_entity_id=a.repair_work_id
join di_disinfo_ro_serv_repair sr on sr.import_entity_id=a.disinfo_ro_srvrep_id
join service.tmp_relation t on t.new_disinfo_ro_id=sr.disinfo_ro_id and create_new;

update di_repair_work_list set import_entity_id= null where object_version=18994;
update di_disinfo_ro_serv_repair set import_entity_id= null where object_version=18994;

delete from DI_DISINFO_RELATION where object_version=18994;");
        }

        /// <inheritdoc />
        public override void Down()
        {
        }
    }
}