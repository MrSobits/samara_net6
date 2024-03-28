namespace Bars.Gkh.RegOperator.ViewManagerViews
{
    using System;

    using Bars.B4.Modules.Ecm7.Framework;
    
    public class RegopViewCollection : BaseGkhViewCollection
    {
        public override int Number
        {
            get { return 4; }
        }

        private string CreateViewRegop(DbmsKind dbmsKind)
        {
            return @"
create or replace view view_regop_personal_account as

select
	pa.id,
	r.id as room_id,
	ro.id as ro_id,
	mu.id as mu_id,
	stl.id as stl_id,

	ro.address,
	mu.name as municipality,
	stl.name as settlement,
	ro.address || ', кв. ' || r.croom_num as room_address,
	fa.place_name,
	fa.street_name,
	r.croom_num,

	case when c.id is not null
		then c.name
		else pao.name
	end as owner_name,
	pao.id as owner_id,
	pao.owner_type,
	pc.id as priv_cat_id,
	
	r.carea,
	pa.acc_num,
	pa.area_share,
	pa.open_date,
	pa.close_date,
	pa.REGOP_PERS_ACC_EXTSYST as external_number,
	
	s.id as state_id,
	case when ch.id is not null then true else false end as has_charges
	
from regop_pers_acc pa
	join gkh_room r on r.id = pa.room_id
	join gkh_reality_object ro on ro.id = r.ro_id
	join b4_fias_address fa on fa.id = ro.fias_address_id
	join gkh_dict_municipality mu on mu.id = ro.municipality_id
	left join gkh_dict_municipality stl on stl.id = ro.STL_MUNICIPALITY_ID
	join REGOP_PERS_ACC_OWNER pao on pao.id = pa.ACC_OWNER_ID
	left join REGOP_PRIVILEGED_CATEGORY pc on pc.id=pao.PRIVILEGED_CATEGORY
	left join REGOP_LEGAL_ACC_OWN lpao on lpao.id=pao.id
	left join gkh_contragent c on c.id = lpao.contragent_id
	left join b4_state s on s.id = pa.state_id
	left join (
		select
			pac.pers_acc_id,
			max(pac.id) as id
		from regop_pers_acc_charge pac
		where pac.charge_date >= (select cstart from REGOP_PERIOD where not cis_closed)
		group by pac.pers_acc_id
	) ch on ch.pers_acc_id = pa.id
";
        }

        private string DeleteViewRegop(DbmsKind dbmsKind)
        {
            var viewName = "view_regop_personal_account";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string CreateViewDebtor(DbmsKind dbmsKind)
        {
            return @"
drop view if exists view_clw_debtor;
create view view_clw_debtor as
select
    ls.id,
    cw.id as clw_id,
    doc.type_document as document_type,
    pao.owner_type,
    coalesce(c.name, pao.name) as owner_name,
    0 as ro_id,--ro.id as ro_id,
    COALESCE(c.juridical_address, ro.address) as address, --ro.address,
    0 as room_id, --r.id as room_id,
    null as room_address, --ro.address || ', кв.' || r.croom_num as room_address,
    0 as mu_id, --mu.id as mu_id,
    null as municipality, --mu.name as municipality,
    0 as  stl_id, --stl.id as stl_id,
    null as settlement, --stl.name as settlement,
    ji.name as jursector_number,
    ls.bid_date as ls_doc_date,
    ls.bid_number as ls_doc_number,
    ls.who_considered,
    ls.date_of_adoption,
    ls.date_of_rewiew,
    ls.result_consideration,
    ls.debt_sum_approv,
    ls.penalty_debt_approv,
    ls.law_type_document,
    ls.date_considerat,
    ls.num_considerat,
    ls.cb_debt_sum,
    ls.cb_penalty_debt as cb_penalty_debt_sum,
    pdoc.document_date as pret_doc_date,
    pdoc.document_number as pret_doc_number,
    pret.sum as pret_sum,
    pret.penalty as pret_penalty_sum,
    case when st.final_state and ((coalesce(cw.debt_paid_date,'30000101') < coalesce(law.date_considerat,'00010101'))
                             or (coalesce(law.date_considerat,'00010101') = '00010101' and (coalesce(cw.debt_paid_date,'30000101') < coalesce(law.cb_date_initiated,'00010101')))
                             or (coalesce(law.date_considerat,'00010101') ='00010101' and coalesce(law.cb_date_initiated,'00010101') = '00010101') ) then 10
         when law is null then 0
         else 20 end as REPAID_BEFORE_DEC,
    case when st.final_state and (coalesce(cw.debt_paid_date,'30000101')<coalesce(law.cb_date_initiated,'00010101') or coalesce(law.cb_date_initiated,'00010101') = '00010101') then 10
             when law is null then 0
         else 20 end as REPAID_BEFORE_EX_PROC,

    ord.id  is not null as Objection

from clw_lawsuit ls
join clw_document doc on doc.id = ls.id
    
left join clw_jur_institution ji on ji.id = ls.jinst_id
join clw_claim_work cw on cw.id = doc.claimwork_id
join clw_debtor_claim_work dcw on dcw.id = cw.id
left join b4_state st on st.id=cw.state_id

join regop_pers_acc_owner pao on pao.id = dcw.OWNER_ID
left join regop_legal_acc_own lao on lao.id = pao.id
left join gkh_contragent c on c.id = lao.contragent_id
left join regop_individual_acc_own iao on iao.id = pao.id
left join gkh_reality_object ro on ro.id = iao.registration_ro_id

left join clw_document pdoc on pdoc.claimwork_id = cw.id and pdoc.type_document=20
left join clw_pretension pret on pret.id = pdoc.id

left join clw_document ldoc on ldoc.claimwork_id=cw.id and ldoc.type_document=30
left join clw_lawsuit law on law.id=ldoc.id
left join clw_court_order_claim ord on law.id = ord.id";
        }

        private string DeleteViewDebtor(DbmsKind dbmsKind)
        {
            var viewName = "view_clw_debtor";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string CreateViewDebtorExport(DbmsKind dbmsKind)
        {
            return @"drop view if exists view_clw_debtor_export;
                     create view view_clw_debtor_export as
                     select
                         deb.id,
                         deb.account_id as pers_acc_id,
                         mun.id as municipality_id,
                         mun.name as municipality_name,
                         stl.name as settlement,
                         CONCAT(ro.address, ', кв.', gr.croom_num) as room_address,
                         acc.state_id as state_id,
                         st.name as state_name,
                         acc.acc_num as pers_acc_num,
                         acc.acc_owner_id as owner_id,
                         case when own.owner_type = 0
                              then own.name
                              when own.owner_type = 1
                              then cont.name
                         else '' 
                         end as owner_name,
                         own.owner_type as owner_type,
                         deb.debt_sum as debt_sum,
                         deb.debt_base_tariff_sum as base_tariff_sum,
                         deb.debt_decision_tariff_sum as decision_tariff_sum,
                         deb.days_count as expiration_days_count,
                         deb.month_count as expiration_month_count,
                         deb.penalty_debt as penalty_debt,
                         case when clw is not null
                              then true
                         else false 
                         end as has_claimwork,
                         deb.court_type as court_type,
                         ji.short_name as jur_inst,
                         us.name as user_name,
                         ro.id as ro_id,
                         deb.extract_exists as extract_exists,
                         deb.rosreg_acc_matched as acc_rosreg_match,
                         case when iown is not null and iown.birth_date is not null and (current_date - iown.birth_date::date) < 6574
                              then true
                         else false 
                         end as underage,
                         (gr.carea * acc.area_share) as owner_area,
                         gr.carea as room_area,
                         deb.processed_by_agent as processed_dy_agent,
                         case when (gr.carea * acc.area_share) = gr.carea
                              then 20
                         else 10 
                         end as separate,
                         deb.claim_work_id as claimwork_id,
                         deb.lastclw_debt_sum as last_clw_debt,
                         deb.payments_sum as payments_sum,
                         deb.new_claim_debt as new_debt,
                         deb.last_debt_period as last_clw_period
                         
                         from regop_debtor deb
                             join regop_pers_acc acc on acc.id = deb.account_id

                             join gkh_room gr on gr.id = acc.room_id
                             join gkh_reality_object ro on ro.id = gr.ro_id
                             join gkh_dict_municipality mun on mun.id = ro.municipality_id
                             join gkh_dict_municipality stl on stl.id = ro.stl_municipality_id

                             join regop_pers_acc_owner own on own.id = acc.acc_owner_id
                             left join regop_individual_acc_own iown on iown.id = own.id

                             left join regop_legal_acc_own lown on lown.id = own.id
                             left join gkh_contragent cont on cont.id = lown.contragent_id

                             left join clw_claim_work_acc_detail clw on clw.account_id = deb.account_id
                             left join clw_jur_institution ji on ji.id = deb.jur_inst_id
                             left join clw_debtor_claim_work dclw on dclw.id = clw.claim_work_id
                             left join b4_user us on us.id = dclw.user_id
                             left join b4_state st on st.id = acc.state_id";
        }

        private string DeleteViewDebtorExport(DbmsKind dbmsKind)
        {
            var viewName = "view_clw_debtor_export";

            return DropViewPostgreQuery(viewName);
        }

        private string CreateViewDocumentClw(DbmsKind dbmsKind)
        {
            return @"
DROP VIEW IF EXISTS view_clw_document_register;
CREATE VIEW view_clw_document_register as
SELECT
    doc.id as ID,
    w.real_obj_id as RO_ID,
    w.id as CLW_ID,
    doc.type_document as DOC_TYPE,
    w.type_base as BASE_TYPE,
    w.base_info as BASE_INFO,
    COALESCE(dw.debtor_type, 0) as DEBTOR_TYPE,
    COALESCE(c.juridical_address, ro.address) as ADDRESS,
    mun.name as MUNICIPALITY,
    doc.document_date::date as DOCUMENT_DATE,
    COALESCE(notif.send_date, pretens.review_date, law.date_of_rewiew)::date as REVIEW_DATE,

    pretens.sum as PRETENS_SUM,
    pretens.penalty as PRETENS_PENALTY,
        CASE dw.debtor_type
            WHEN 1 THEN doc.document_date::date + COALESCE((SELECT COALESCE(encode(value, 'escape')::integer, 0) FROM gkh_config_parameter WHERE key = 'DebtorClaimWork.Individual.Pretension.PaymentPlannedPeriod'), 0)
            WHEN 2 THEN doc.document_date::date + COALESCE((SELECT COALESCE(encode(value, 'escape')::integer, 0) FROM gkh_config_parameter WHERE key = 'DebtorClaimWork.Legal.Pretension.PaymentPlannedPeriod'), 0)
            ELSE null
        END as PRETENS_PLANED_DATE,

    law.bid_number as LAW_BID_NUMBER,
    law.bid_date::date as LAW_BID_DATE,
    law.cb_debt_sum as LAW_DEBT_SUM,
    law.cb_penalty_debt as LAW_PENALTY_DEBT
FROM clw_document doc
JOIN clw_claim_work w ON w.id = doc.claimwork_id
LEFT JOIN clw_debtor_claim_work dw ON dw. id = w.id

LEFT JOIN clw_individual_claim_work ind ON ind. id = w.id
LEFT JOIN clw_legal_claim_work leg ON leg. id = w.id
LEFT JOIN regop_pers_acc_owner pao ON pao.id = dw.owner_id
LEFT JOIN regop_legal_acc_own lao ON lao.id = pao.id
LEFT JOIN regop_individual_acc_own iao ON iao.id = pao.id
LEFT JOIN gkh_reality_object ro ON ro.id = iao.registration_ro_id

LEFT JOIN clw_build_claim_work bw ON bw. id = w.id
LEFT JOIN cr_obj_build_contract obc ON obc.id = bw.contract_id
LEFT JOIN gkh_builder b ON b.id = obc.builder_id

LEFT JOIN gkh_contragent c ON c.id = lao.contragent_id OR c.id = b.contragent_id
LEFT JOIN gkh_dict_municipality mun ON mun.id = c.municipality_id OR mun.id = ro.municipality_id

LEFT JOIN clw_notification notif ON notif.id = doc.id
LEFT JOIN clw_pretension pretens ON pretens.id = doc.id
LEFT JOIN (clw_petition p
    JOIN clw_lawsuit law ON law.id = p.id
) ON p.id = doc.id;
";
        }

        private string DeleteViewDocumentClw(DbmsKind dbmsKind)
        {
            return "DROP VIEW IF EXISTS view_clw_document_register;";
        }

        private string CreateViewIndividualClaimWork(DbmsKind dbmsKind)
        {
            return @"
            DROP VIEW IF EXISTS view_clw_individual_claim_work;
            CREATE VIEW view_clw_individual_claim_work as
            SELECT * FROM GetViewIndividualClaimWork();";
        }

        private string DeleteViewIndividualClaimWork(DbmsKind dbmsKind)
        {
            var viewName = "VIEW_CLW_INDIVIDUAL_CLAIM_WORK";

            return this.DropViewPostgreQuery(viewName);
        }

        private string CreateFunctionGetViewIndividualClaimWork(DbmsKind dbmsKind)
        {
            return @"
            DROP FUNCTION IF EXISTS GetViewIndividualClaimWork();
            CREATE FUNCTION GetViewIndividualClaimWork () RETURNS TABLE (
                ""id"" int8,
                ""debtor_state"" int4,
                ""state_id"" int8,
                ""municipality"" VARCHAR,
                ""registration_address"" VARCHAR,
                ""account_owner_name"" VARCHAR,
                ""accounts_number"" int4,
                ""accounts_address"" VARCHAR,
                ""cur_charge_base_tariff_debt"" NUMERIC,
                ""cur_charge_decision_tariff_debt"" NUMERIC,
                ""cur_charge_debt"" NUMERIC,
                ""cur_penalty_debt"" NUMERIC,
                ""is_debt_paid"" bool,
                ""debt_paid_date"" DATE,
                ""user_id"" int8,
                ""object_create_date"" DATE
                ) AS $BODY$ DECLARE
                rec record;
            BEGIN

            DROP TABLE IF EXISTS mun;
            CREATE TEMP TABLE mun AS SELECT
            i.ID,
            M.NAME 
            FROM CLW_INDIVIDUAL_CLAIM_WORK i
                JOIN CLW_CLAIM_WORK_ACC_DETAIL cd ON i.ID = cd.claim_work_id
                JOIN regop_pers_acc pa ON pa.ID = cd.account_id
                JOIN gkh_room gr ON gr.ID = pa.room_id
                JOIN gkh_reality_object r ON r.ID = gr.ro_id
                JOIN gkh_dict_municipality M ON M.ID = r.municipality_id 
            GROUP BY
                i.ID,
                M.NAME;
            CREATE INDEX ind_mun ON mun ( ID );
            ANALYZE mun;

            DROP TABLE IF EXISTS works;
            CREATE TEMP TABLE works AS SELECT
            cwd.claim_work_id,
            string_agg (( gro.address || ', кв. ' || rm.croom_num ) :: TEXT, ', ' ) ad 
            FROM CLW_CLAIM_WORK_ACC_DETAIL cwd
                JOIN REGOP_PERS_ACC ac ON ac.ID = cwd.account_id
                JOIN GKH_ROOM rm ON rm.ID = ac.room_id
                JOIN GKH_REALITY_OBJECT gro ON gro.ID = rm.ro_id 
            GROUP BY 1;
            CREATE INDEX ind_works ON works ( claim_work_id );
            ANALYZE works;

            DROP TABLE IF EXISTS svod;
            CREATE TEMP TABLE svod AS SELECT
            cw.ID AS ID,
            cbw.debtor_state as debtor_state,
            cw.state_id AS state_id,
            COALESCE (
            mu.NAME,
            (
            CASE
    
                WHEN ( SELECT COUNT ( * ) FROM mun WHERE mun.ID = icw.ID ) = 1 THEN
                ( SELECT mun.NAME FROM mun WHERE mun.ID = icw.ID ) ELSE'' 
            END 
                )) AS municipality,
                COALESCE ( ro.address, '' ) AS registration_address,
                own.NAME AS account_owner_name,
                ( SELECT COUNT ( * ) FROM CLW_CLAIM_WORK_ACC_DETAIL cwd WHERE cwd.claim_work_id = icw.ID ) AS accounts_number,
                works.ad AS accounts_address,
                cbw.CUR_CHARGE_BASE_TARIFF_DEBT,
                cbw.CUR_CHARGE_DECISION_TARIFF_DEBT,
                cbw.CUR_CHARGE_DEBT,
                cbw.CUR_PENALTY_DEBT,
                cw.IS_DEBT_PAID,
                cw.DEBT_PAID_DATE,
                cbw.user_id AS user_id,
                cw.object_create_date AS object_create_date
            FROM CLW_CLAIM_WORK cw
                JOIN CLW_DEBTOR_CLAIM_WORK cbw ON cw.ID = cbw.
                ID JOIN ( CLW_INDIVIDUAL_CLAIM_WORK icw JOIN works ON works.claim_work_id = icw.ID ) ON icw.ID = cbw.
                ID JOIN REGOP_PERS_ACC_OWNER own ON own.ID = cbw.owner_id
                JOIN REGOP_INDIVIDUAL_ACC_OWN iown ON iown.ID = own.
                ID LEFT JOIN GKH_REALITY_OBJECT ro ON ro.ID = iown.registration_ro_id
                LEFT JOIN GKH_DICT_MUNICIPALITY mu ON mu.ID = ro.municipality_id;

            FOR rec IN EXECUTE'select * from svod'
            loop
            ID = rec.ID;
            debtor_state = rec.debtor_state;
            state_id = rec.state_id;
            municipality = rec.municipality;
            registration_address = rec.registration_address;
            account_owner_name = rec.account_owner_name;
            accounts_number = rec.accounts_number;
            accounts_address = rec.accounts_address;
            CUR_CHARGE_BASE_TARIFF_DEBT = rec.CUR_CHARGE_BASE_TARIFF_DEBT;
            CUR_CHARGE_DECISION_TARIFF_DEBT = rec.CUR_CHARGE_DECISION_TARIFF_DEBT;
            CUR_CHARGE_DEBT = rec.CUR_CHARGE_DEBT;
            CUR_PENALTY_DEBT = rec.CUR_PENALTY_DEBT;
            IS_DEBT_PAID = rec.IS_DEBT_PAID;
            DEBT_PAID_DATE = rec.DEBT_PAID_DATE;
            user_id = rec.user_id;
            object_create_date = rec.object_create_date;
            RETURN NEXT;

            END loop;
            END;
            $BODY$ LANGUAGE plpgsql VOLATILE COST 100 ROWS 1000;
";
        }

        private string DeleteFunctionGetViewIndividualClaimWork(DbmsKind dbmsKind)
        {
            return "DROP FUNCTION IF EXISTS GetViewIndividualClaimWork();";
        }

        private string CreateViewZRegop(DbmsKind dbmsKind)
        {
            return @"CREATE OR REPLACE VIEW z_view_regop_pa AS 
                        SELECT pa.id,
                        r.id AS room_id,
                        ro.id AS ro_id,
                        mu.id AS mu_id,
                        stl.id AS stl_id,
                        ro.address,
                        mu.name AS municipality,
                        stl.name AS settlement,
                        (ro.address::text || ', кв. '::text) || r.croom_num::text AS room_address,
                        fa.place_name,
                        fa.street_name,
                        r.croom_num,
                            CASE
                                WHEN c.id IS NOT NULL THEN c.name
                                ELSE pao.name
                            END AS owner_name,
                        pao.id AS owner_id,
                        pao.owner_type,
                        pc.id AS priv_cat_id,
                        r.carea,
                        pa.acc_num,
                        pa.area_share,
                        pa.open_date,
                        pa.close_date,
                        pa.regop_pers_acc_extsyst AS external_number,
                        pa.state_id
                        FROM regop_pers_acc pa
                            JOIN gkh_room r ON r.id = pa.room_id
                            JOIN gkh_reality_object ro ON ro.id = r.ro_id
                            JOIN b4_fias_address fa ON fa.id = ro.fias_address_id
                            JOIN gkh_dict_municipality mu ON mu.id = ro.municipality_id
                            LEFT JOIN gkh_dict_municipality stl ON stl.id = ro.stl_municipality_id
                            JOIN regop_pers_acc_owner pao ON pao.id = pa.acc_owner_id
                            LEFT JOIN regop_privileged_category pc ON pc.id = pao.privileged_category
                            LEFT JOIN regop_legal_acc_own lpao ON lpao.id = pao.id
                            LEFT JOIN gkh_contragent c ON c.id = lpao.contragent_id
                            LEFT JOIN b4_state s ON s.id = pa.state_id;";
        }

        private string CreateCheckRecalcParams(DbmsKind dbmsKind)
        {
            return @"CREATE OR REPLACE FUNCTION public.check_recalc_params
                                                (
                                                    IN _period_id integer,
                                                    IN _is_need_recalc_ls boolean DEFAULT false::boolean,
                                                    IN _is_need_deny_recalc_ls boolean DEFAULT false::boolean,
                                                    IN _area numeric DEFAULT 0::numeric,
                                                    IN _tariff numeric DEFAULT 0::numeric,
                                                    IN _list_mo character varying DEFAULT ''::character varying
                                                )
                                                RETURNS TABLE
                                                (
                                                    acc_num             character varying,
                                                    state               character varying,
                                                    mo_name             character varying,
                                                    mr_name             character varying,
                                                    address             character varying,
                                                    ls_type             character varying,
                                                    ls_fio              character varying,
                                                    area                numeric,
                                                    tariff              numeric,
                                                    area_share          numeric,
                                                    formation_variant   character varying,
                                                    recalc_auto         character varying,
                                                    recalc_manual       character varying,
                                                    recalc_ban          character varying
                                                ) AS
        $BODY$
        declare
            _open_period_start date;
                        _period_start date;
                        _period_end date;
                        _is_closed boolean;
                        begin
                            -- Выбираем дату начала и дату конца выбранного периода
            select cstart, coalesce(cend, cstart +interval '1 month' - interval '1 day'), cis_closed
                into _period_start, _period_end, _is_closed
                from regop_period
                where id = _period_id;

                        --Выбираем дату начала открытого периода
            select cstart
                into _open_period_start
                from regop_period
                where cis_closed = false;

                        create temp table _house_calc(fond_types int);

                        --Читаем настройки способа формирования фонда для фильтра в выборке--
            if (select encode(conf.value, 'escape')::boolean from gkh_config_parameter conf where conf.key = 'RegOperator.GeneralConfig.HouseCalculationConfig.RegopSpecialCalcAccount') = true
            then insert into _house_calc values(0); end if;
                        if (select encode(conf.value, 'escape')::boolean from gkh_config_parameter conf where conf.key = 'RegOperator.GeneralConfig.HouseCalculationConfig.RegopCalcAccount') = true
            then insert into _house_calc values(1); end if;
                        if (select encode(conf.value, 'escape')::boolean from gkh_config_parameter conf where conf.key = 'RegOperator.GeneralConfig.HouseCalculationConfig.SpecialCalcAccount') = true
            then insert into _house_calc values(2); end if;
                        if (select encode(conf.value, 'escape')::boolean from gkh_config_parameter conf where conf.key = 'RegOperator.GeneralConfig.HouseCalculationConfig.Unknown') = true
            then insert into _house_calc values(3); end if;

                        --Создаем результирующую таблицу
            create temp table _result_table(acc_id bigint, --ключик для таблицы логов
                                                ro_id integer, --ключик для вычисления тарифа
                                                acc_owner_id bigint,
                                                acc_num character varying,
                                                state character varying,
                                                mo_name character varying,
                                                mr_name character varying,
                                                address character varying,
                                                ls_type character varying,
                                                ls_fio character varying,
                                                area numeric,
                                                tariff numeric,
                                                area_share numeric,
                                                formation_variant character varying,
                                                recalc_auto character varying,
                                                recalc_manual character varying,
                                                recalc_ban character varying);


                    --Заполняем результирующую таблицу--
            if _is_closed = false then
                insert into _result_table
                select pa.id,
                    ro.id,
                    pa.acc_owner_id,
                    pa.acc_num, 
                    st.name, 
                    mo.name as mo_name, 
                    mo2.name as mr_name, 
                    ro.address, 
                    case ow.owner_type when 0 then 'Физ. лицо' when 1 then 'Юр. лицо' else '' end ls_type,
                    '',
                    (tr.param_values::json->> 'room_area')::numeric,
                    (tr.param_values::json->> 'base_tariff')::numeric,
                    (tr.param_values::json->> 'area_share')::numeric,
                    case when ro.acc_form_variant = 0
                    then 'Специальный счет регионального оператора'::character varying
                    when ro.acc_form_variant = 1
                    then 'Счет регионального оператора'::character varying
                    when ro.acc_form_variant = 2
                    then 'Специальный счет'::character varying
                    when ro.acc_form_variant = 3
                    then 'Не выбран'::character varying end formation_variant,
                '',
                '',
                ''
            from regop_pers_acc as pa
                left join gkh_room as room
                    on pa.room_id = room.id
                    inner join gkh_reality_object as ro
                        on room.ro_id = ro.id
                        and ro.condition_house not in (10, 40)                                  --Целый дом, не 10 - аварийный и не 40 - снесённый
                        and ro.is_not_involved_cr = false-- Участвует в программе капремонта
                        and ro.acc_form_variant in (select fond_types from _house_calc)
                inner join gkh_dict_municipality as mo                                          --Только заданные в условии муниципальные образования
                    on ro.stl_municipality_id = mo.id
                inner join gkh_dict_municipality as mo2
                    on ro.municipality_id = mo2.id
                inner join regop_pers_acc_owner as ow
                    on pa.acc_owner_id = ow.id
                inner join b4_state as st
                    on pa.state_id = st.id
                inner join regop_pers_acc_charge as ch
                    on pa.id = ch.pers_acc_id
                    and charge_date between _period_start and _period_end
                inner join regop_calc_param_trace as tr
                    on ch.guid = tr.calc_guid
                    and tr.calc_type = 0-- Тип начисления
            where(coalesce(_list_mo, '') = '' or _list_mo = 'All' or mo.id in (select cast(mo as int) from regexp_split_to_table(_list_mo, ',') as mo))
            and st.name = 'Открыт'
            or(st.name = 'Закрыт' and pa.close_date > _open_period_start)
            or(st.name = 'Закрыт с долгом' and pa.close_date > _open_period_start);
        else
                insert into _result_table
                select pa.id,
                    ro.id,
                    pa.acc_owner_id,
                    pa.acc_num, 
                    st.name, 
                    mo.name as mo_name, 
                    mo2.name as mr_name, 
                    ro.address, 
                    case ow.owner_type when 0 then 'Физ. лицо' when 1 then 'Юр. лицо' else '' end ls_type,
                    '',
                    room.carea,
                    z_get_tariff(ro.id),
                    pa.area_share,
                    case when ro.acc_form_variant = 0
                    then 'Специальный счет регионального оператора'::character varying
                    when ro.acc_form_variant = 1
                    then 'Счет регионального оператора'::character varying
                    when ro.acc_form_variant = 2
                    then 'Специальный счет'::character varying
                    when ro.acc_form_variant = 3
                    then 'Не выбран'::character varying end formation_variant,
                '',
                '',
                ''
            from regop_pers_acc as pa
                inner join gkh_room as room
                    on pa.room_id = room.id
                    inner join gkh_reality_object as ro
                        on room.ro_id = ro.id
                        and ro.condition_house not in (10, 40)
                        and ro.is_not_involved_cr = false
                        and ro.acc_form_variant in (select fond_types from _house_calc)
                inner join gkh_dict_municipality as mo
                    on ro.stl_municipality_id = mo.id
                inner join gkh_dict_municipality as mo2
                    on ro.municipality_id = mo2.id
                inner join regop_pers_acc_owner as ow
                    on pa.acc_owner_id = ow.id
                inner join b4_state as st
                    on pa.state_id = st.id
                left join regop_cashpay_pers_acc cashacc
                    on pa.id = cashacc.pers_acc_id
                    inner join regop_cashpayment_center cashc
                        on cashacc.cashpaym_center_id = cashc.id
                        and cashc.conductsaccrual = false
            where(coalesce(_list_mo, '') = '' or _list_mo = 'All' or mo.id in (select cast(mo as int) from regexp_split_to_table(_list_mo, ',') as mo))
            and st.name = 'Открыт'
            or(st.name = 'Закрыт' and pa.close_date > _open_period_start)
            or(st.name = 'Закрыт с долгом' and pa.close_date > _open_period_start);
                    end if;
                    --Конец--

                        --Проставляем ownerName
            update _result_table rt
            set ls_fio = indow.surname || ' ' || indow.first_name || ' ' || indow.second_name
            from regop_pers_acc_owner as ow
                inner join regop_individual_acc_own indow
                        on ow.id = indow.id
            where rt.ls_type = 'Физ. лицо'
            and rt.acc_owner_id = ow.id;

                        update _result_table rt
                        set ls_fio = contr.name
            from regop_pers_acc_owner as ow
                inner join regop_legal_acc_own low
                        on ow.id = low.id
                        inner join gkh_contragent contr
                            on low.contragent_id = contr.id
            where rt.ls_type = 'Юр. лицо'
            and rt.acc_owner_id = ow.id;

                        --Фильтруем выборку по протоколам решения у котороых не указано ""Ведение ЛС собственниками""
            create temp table _max_date_protocol(ro_id int, max_date date);
                        insert into _max_date_protocol(ro_id, max_date)
                select pr.ro_id, max(pr.date_start)
                    from gkh_obj_d_protocol pr
                        inner
                    join _result_table rt

                    on pr.ro_id = rt.ro_id
                    group by pr.ro_id;

                        create temp table _max_id_date_protocol(pr_id bigint);
                        insert into _max_id_date_protocol(pr_id)
                select max(pr.id)
                    from gkh_obj_d_protocol as pr
                        inner join _max_date_protocol as mx
                            on mx.ro_id = pr.ro_id
                                and mx.max_date = pr.date_start
                        group by pr.ro_id;


                        create temp table _max_ul_decision(dec_id bigint);

                        insert into _max_ul_decision
                        select max(ul.id)
            from dec_ultimate_decision as ul
                inner join _max_id_date_protocol mx
                    on ul.protocol_id = mx.pr_id
            group by mx.pr_id;

                        create temp table _bad_ro_id(id bigint);

                        insert into _bad_ro_id
                        select pr.ro_id
                        from gkh_obj_d_protocol pr
                inner join _max_id_date_protocol as max
                    on pr.id = max.pr_id
                    inner join dec_ultimate_decision as ul
                        on pr.id = ul.protocol_id
                        inner join _max_ul_decision maxul
                            on maxul.dec_id = ul.id
                    inner join dec_account_manage as accman
                        on ul.id = accman.id
                        and accman.decision = 10; --ведение лс собственниками

            delete from _result_table using _bad_ro_id
            where ro_id = _bad_ro_id.id;

                    --Заполняем признак автоматического перерасчета--
            if _is_closed = false and _is_need_recalc_ls = true then
            update _result_table rt
            set recalc_auto =
            case when rt.acc_id in (select distinct res.acc_id
            from _result_table res
                    inner join gkh_entity_log_light log
                    on res.acc_id = log.entity_id
                        and log.param_name in ('room_area', 'area_share', 'account_open_date', 'account_close_date')
                    inner join regop_pers_acc_calc_param calc
                    on log.id = calc.logged_entity_id) then 'Да' else 'Нет' end;
                    end if;
                    --Конец--

                    --Заполняем признак ручного перерасчета--
            if _is_closed = false and _is_need_recalc_ls = true then
            update _result_table rt
            set recalc_manual =
            case when rt.acc_id in (select distinct res.acc_id
            from _result_table res
                    inner join regop_pers_acc_change ch
                    on res.acc_id = ch.acc_id
                    and ch.change_type = 296 /*Ручной перерасчет*/
                    and ch.date::date >= _open_period_start) then 'Да' else 'Нет' end;
                    end if;
                    --Конец--

                    --Заполняем признак запрета перерасчета--
                    update _result_table rt
            set recalc_ban =
            case when rt.acc_id in (select distinct res.acc_id
            from _result_table res
                    inner join gkh_entity_log_light log
                    on res.acc_id = log.entity_id
                    and log.param_name in ('Запрет перерасчета')
                    and extract(month from log.date_actual) between extract(month from _period_start) and extract(month from _period_end)
                    and extract(year from log.date_actual) between extract(year from _period_start) and extract(year from _period_end)) then 'Да' else 'Нет' end;
                    --Конец--


            return query
                    select t.acc_num,
                            t.state,
                            t.mo_name,
                            t.mr_name, 
                            t.address,
                            t.ls_type,
                            t.ls_fio,
                            t.area,
                            t.tariff, 
                            t.area_share,
                            t.formation_variant,
                            t.recalc_auto,
                            t.recalc_manual,
                            t.recalc_ban
                    from _result_table t
                    where t.area > _area
                    and t.tariff >= _tariff
                    and(_is_need_deny_recalc_ls = true or t.recalc_ban = 'Нет')
                    and(_is_need_recalc_ls = true or(_is_need_recalc_ls = false and _is_closed = true) or(t.recalc_auto = 'Нет' and t.recalc_manual = 'Нет'));

                        drop table _result_table;
                        drop table _house_calc;
                        drop table _max_date_protocol;
                        drop table _max_id_date_protocol;
                        drop table _max_ul_decision;
                        drop table _bad_ro_id;
                        end;
        $BODY$
            LANGUAGE plpgsql VOLATILE
            COST 100
            ROWS 1000; ";
        }

        private string DeleteCheckRecalcParams(DbmsKind dbmsKind)
        {
            var functionName = @"check_recalc_params(integer, boolean, boolean, numeric, numeric, character varying)";
            if (dbmsKind == DbmsKind.PostgreSql)
            {
                return this.DropFunctionPostgreQuery(functionName);
            }

            return null;
        }

        private string DeleteViewZRegop(DbmsKind dbmsKind)
        {
            var viewName = "z_view_regop_pa";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string CreateFunctionPenaltyFormula(DbmsKind dbmsKind)
        {
            return @"
DROP FUNCTION IF EXISTS GET_PENALTY_FORMULA(account_id bigint, period_date date, decimals int);
CREATE OR REPLACE FUNCTION GET_PENALTY_FORMULA(account_id bigint, period_date date, decimals int) RETURNS text AS
$BODY$
DECLARE
    tables RECORD;
    formula_res TEXT DEFAULT '';
BEGIN
    DROP TABLE IF EXISTS _acc_formula;
    CREATE TEMP TABLE _acc_formula (id bigint, calc_type int, period_id bigint, start_date timestamp, end_date timestamp, formula text);

    FOR tables IN (
    SELECT 
        'REGOP_CALC_PARAM_TRACE_PERIOD_' || regop_period.id as REGOP_CALC_PARAM_TRACE,
        'REGOP_PERS_ACC_CHARGE_PERIOD_' || regop_period.id as REGOP_PERS_ACC_CHARGE
    FROM regop_period
    ) LOOP
    EXECUTE format($$
-- Тело запроса
INSERT INTO _acc_formula
WITH penalty_info AS (
SELECT
    c.PERS_ACC_ID as Id,
    unnest(array_agg(cpt.DATE_START::date)) as start_date,
    unnest(array_agg(COALESCE(cpt.DATE_END, now())::date)) as end_date,
    unnest(array_agg(round((cpt.PARAM_VALUES::json->'penalty_debt')::TEXT::NUMERIC, %1$s))) as penaltydebt,
    unnest(array_agg(round((cpt.PARAM_VALUES::json->'payment_penalty_percentage')::TEXT::NUMERIC, %1$s))) as percentage,
    cpt.period_id,
    cpt.calc_type
FROM
    %2$s cpt
JOIN %3$s c ON c.GUID = cpt.calc_guid
WHERE
    cpt.CALC_TYPE = 2
AND 
    cpt.DATE_START < %4$L
AND (
    cpt.DATE_END IS NULL
    OR
    cpt.DATE_END < %4$L
)
AND 
    c.pers_acc_id = %5$s
AND
    (cpt.PARAM_VALUES::json->'penalty_debt')::TEXT::NUMERIC > 0
AND 
    (cpt.PARAM_VALUES::json->'payment_penalty_percentage')::TEXT::NUMERIC > 0
AND
    COALESCE(cpt.DATE_END, now())::date - cpt.DATE_START::date + 1 > 0
GROUP BY
    c.pers_acc_id, cpt.period_id, cpt.calc_type

UNION

SELECT
    c.PERS_ACC_ID as Id,
    unnest(array_agg(cpt.DATE_START::date)) as start_date,
    unnest(array_agg(COALESCE(cpt.DATE_END, now())::date)) as end_date,
    unnest(array_agg(round((cpt.PARAM_VALUES::json->'debt')::TEXT::NUMERIC, %1$s))) as penaltydebt,
    unnest(array_agg(round((cpt.PARAM_VALUES::json->'recalc_percent')::TEXT::NUMERIC, %1$s))) as percentage,
    cpt.period_id,
    cpt.calc_type
FROM
    %2$s cpt
JOIN %3$s c ON c.GUID = cpt.calc_guid
WHERE
    cpt.CALC_TYPE = 5
AND 
    cpt.DATE_START < %4$L
AND (
    cpt.DATE_END IS NULL
    OR
    cpt.DATE_END < %4$L
)
AND 
    c.pers_acc_id = %5$s
AND
    (cpt.PARAM_VALUES::json->'debt')::TEXT::NUMERIC > 0
AND 
    (cpt.PARAM_VALUES::json->'recalc_percent')::TEXT::NUMERIC > 0
AND
    COALESCE(cpt.DATE_END, now())::date - cpt.DATE_START::date + 1 > 0
GROUP BY
    c.pers_acc_id, cpt.period_id, cpt.calc_type
)
SELECT 
    id, 
    calc_type,
    period_id,
    start_date,
    end_date,
    coalesce(string_agg('(' || PENALTYDEBT || ' * (' || PERCENTAGE || ' / 100) / 300 * ' || END_DATE - START_DATE + 1 || ')', ' + ')::text,'')
FROM penalty_info
GROUP BY id, calc_type, period_id, start_date, end_date
ORDER BY start_date;

$$, decimals, tables.REGOP_CALC_PARAM_TRACE, tables.REGOP_PERS_ACC_CHARGE, period_date, account_id);
    END LOOP;

    SELECT string_agg(p.formula, ' + ') formula
    FROM _acc_formula p
    WHERE p.formula IS NOT NULL
	AND case when p.calc_type = 2 then not exists(select p1.id from _acc_formula p1 where p1.calc_type = 5 and p1.start_date >= p.start_date and p1.end_date <= p.END_DATE and p1.id = p.id)
	 when p.calc_type = 5 then not exists(select p1.id from _acc_formula p1 where p1.calc_type = 5 and p1.start_date >= p.start_date and p1.end_date <= p.END_DATE and p1.period_id > p.period_id and p1.period_id <> p.period_id and p1.id = p.id)
	 else false
	 end
    INTO formula_res;

    DROP TABLE IF EXISTS _acc_formula;

    RETURN formula_res;
END;
$BODY$
LANGUAGE plpgsql;
";
        }

        private string DeleteFunctionPenaltyFormula(DbmsKind dbmsKind)
        {
            return "DROP FUNCTION IF EXISTS GET_PENALTY_FORMULA(account_id bigint, period_date date, decimals int);";
        }

        private string CreateViewAnalitRegopArea(DbmsKind dbmsKind)
        {
            return @"
DROP VIEW IF EXISTS view_analit_reg_op_area;
CREATE OR REPLACE VIEW view_analit_reg_op_area AS 
 SELECT ro.id AS code,
    mu.name AS raion,
    fi.address_name AS address,
    ro.area_liv_not_liv_mkd,
    rooms.arearooms,
    schet.roomareashare,
    ro.number_apartments,
    rooms.countrooms,
    schet.countschet
   FROM gkh_reality_object ro
     LEFT JOIN gkh_dict_municipality mu ON mu.id = ro.municipality_id
     LEFT JOIN b4_fias_address fi ON fi.id = ro.fias_address_id
     LEFT JOIN ( SELECT DISTINCT dpkr_1.ro_id AS id
           FROM ovrhl_version_rec dpkr_1
             LEFT JOIN ovrhl_prg_version v ON v.id = dpkr_1.version_id
          WHERE v.is_main) dpkr ON dpkr.id = ro.id
     LEFT JOIN ( SELECT gkh_room.ro_id,
            count(gkh_room.id) AS countrooms,
            sum(gkh_room.carea) AS arearooms
           FROM gkh_room
          GROUP BY gkh_room.ro_id) rooms ON rooms.ro_id = ro.id
     LEFT JOIN ( SELECT h.ro_id,
            sum(h.roomareashare) AS roomareashare,
            sum(h.countschet) AS countschet
           FROM ( SELECT count(schet_1.acc_num) AS countschet,
                    sum(schet_1.area_share) AS shareschet,
                    room.carea * sum(schet_1.area_share) AS roomareashare,
                    room.id,
                    room.carea AS roomarea,
                    room.ro_id
                   FROM regop_pers_acc schet_1
                     LEFT JOIN regop_pers_acc_owner abonent ON abonent.id = schet_1.acc_owner_id
                     LEFT JOIN b4_state status ON status.id = schet_1.state_id
                     LEFT JOIN gkh_room room ON room.id = schet_1.room_id
                  WHERE status.name::text = 'Открыт'::text
                  GROUP BY room.id) h
          GROUP BY h.ro_id) schet ON schet.ro_id = ro.id
  WHERE dpkr.id IS NOT NULL;
";
        }

        private string CreateViewRegopPa(DbmsKind dmsKind)
        {
            return @"CREATE OR REPLACE VIEW z_view_regop_pa AS 
 SELECT pa.id,
    r.id AS room_id,
    ro.id AS ro_id,
    mu.id AS mu_id,
    stl.id AS stl_id,
    ro.address,
    mu.name AS municipality,
    stl.name AS settlement,
    (ro.address::text || ', кв. '::text) || r.croom_num::text AS room_address,
    fa.place_name,
    fa.street_name,
    r.croom_num,
        CASE
            WHEN c.id IS NOT NULL THEN c.name
            ELSE pao.name
        END AS owner_name,
    pao.id AS owner_id,
    pao.owner_type,
    pc.id AS priv_cat_id,
    r.carea,
    pa.acc_num,
    pa.area_share,
    pa.open_date,
    pa.close_date,
    pa.regop_pers_acc_extsyst AS external_number,
    pa.state_id
   FROM regop_pers_acc pa
     JOIN gkh_room r ON r.id = pa.room_id
     JOIN gkh_reality_object ro ON ro.id = r.ro_id
     JOIN b4_fias_address fa ON fa.id = ro.fias_address_id
     JOIN gkh_dict_municipality mu ON mu.id = ro.municipality_id
     LEFT JOIN gkh_dict_municipality stl ON stl.id = ro.stl_municipality_id
     JOIN regop_pers_acc_owner pao ON pao.id = pa.acc_owner_id
     LEFT JOIN regop_privileged_category pc ON pc.id = pao.privileged_category
     LEFT JOIN regop_legal_acc_own lpao ON lpao.id = pao.id
     LEFT JOIN gkh_contragent c ON c.id = lpao.contragent_id
     LEFT JOIN b4_state s ON s.id = pa.state_id;

ALTER TABLE z_view_regop_pa
  OWNER TO bars;";
        }

        private string DeleteViewAnalitRegopArea(DbmsKind dbmsKind)
        {
            return "DROP VIEW IF EXISTS view_analit_reg_op_area;";
        }

        private string CreateViewRegopOwnershipHistory(DbmsKind dmsKind)
        {
            return @"
DROP VIEW IF EXISTS view_regop_pers_acc_owner;
CREATE OR REPLACE VIEW view_regop_pers_acc_owner AS
    SELECT a.id, a.account_id, a.owner_id, p.id as period_id
        FROM regop_pers_acc_ownership_history a
        JOIN regop_period p ON a.date < COALESCE(p.cend, 'infinity')
        LEFT OUTER JOIN regop_pers_acc_ownership_history b
            ON a.account_id = b.account_id AND a.date < b.date AND b.date < COALESCE(p.cend, 'infinity')
        WHERE b.account_id IS NULL";
        }

        private string DeleteViewRegopOwnershipHistory(DbmsKind dmsKind)
        {
            return "DROP VIEW IF EXISTS view_regop_pers_acc_owner;";
        }
    }
}