namespace Bars.GisIntegration.RegOp.Migrations.Version_2016101200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016101200")]
    [MigrationDependsOn(typeof(Version_2016100700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.ExecuteNonQuery(@"
            //    CREATE OR REPLACE FUNCTION ris.get_external_id(
            //        account_id integer,
            //        in_date date)
            //      RETURNS bigint AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Сгенерировать external_id по дате и коду ЛС
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 06.10.2016                                     
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    BEGIN
            //      return (to_char(in_date, 'yyyyMMdd') || account_id)::bigint;
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql VOLATILE
            //      COST 100;
            //    ALTER FUNCTION ris.get_external_id(integer, date)
            //      OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.get_gkh_contragent_id(ris_contragent_id integer)
            //      RETURNS integer AS
            //    $BODY$
            //                    --=================================================================
            //                    -- Назначение: Получить id контрагента из gkh_contragent по ris_contragent_id из gi_contragent
            //                    -- Автор: Царегородцева Е.Д.                                               
            //                    -- Дата создания: 06.10.2016                                       
            //                    -- Дата изменения:                                      
            //                    --=================================================================
            //                    DECLARE 
            //                      gkhid integer;
            //                    BEGIN
            //                      -- код регопа в МЖФ 
            //                      select g.gkhid into gkhid  from public.gi_contragent g where g.id = ris_contragent_id;
            //                      gkhid := COALESCE(gkhid, 0);   
            //                      return gkhid;
            //                    END;
            //                    $BODY$
            //      LANGUAGE plpgsql VOLATILE
            //      COST 100;
            //    ALTER FUNCTION ris.get_gkh_contragent_id(integer)
            //      OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.get_gkh_house_id(ris_house_id integer)
            //        RETURNS integer AS
            //    $BODY$
            //                    --=================================================================
            //                    -- Назначение: Получить id контрагента из gkh_contragent по ris_contragent_id из gi_contragent
            //                    -- Автор: Царегородцева Е.Д.                                               
            //                    -- Дата создания: 06.10.2016                                       
            //                    -- Дата изменения:                                      
            //                    --=================================================================
            //                    DECLARE 
            //                        house_id integer;
            //                    BEGIN
            //                        select h.external_id into house_id from public.ris_house h where h.id = ris_house_id;
            //                        house_id := COALESCE(house_id, 0);   
            //                        return house_id;
            //                    END;
            //                    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.get_gkh_house_id(integer)
            //        OWNER TO bars;


            //    CREATE OR REPLACE FUNCTION ris.get_regop_period_id(calc_month date)
            //        RETURNS integer AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Получить id периода по месяцу
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 06.10.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE 
            //        period_id integer;
            //    BEGIN
            //        select id INTO period_id from regop_period where calc_month between cstart and cend order by cstart desc, cend desc, id desc limit 1;
            //        period_id := COALESCE(period_id, 0);   
            //        return period_id;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.get_regop_period_id(date)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_select_account(
            //        IN tmp_account text,
            //        IN in_ris_contragent_id integer,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Выбор ЛС
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 04.10.2016                                       
            //    -- Дата изменения: 11.10.2016                                     
            //    --=================================================================
            //    DECLARE
            //        sql text;

            //        gkh_contragent_id integer;
            //        gkh_house_id     integer;
            //    BEGIN
            //        in_ris_house_id := coalesce(in_ris_house_id, 0);
            //        in_ris_contragent_id := coalesce(in_ris_contragent_id, 0);

            //        GKH_CONTRAGENT_ID := ris.get_gkh_contragent_id(in_ris_contragent_id);
            //        gkh_house_id := ris.get_gkh_house_id(in_ris_house_id);

            //        sql := 'CREATE TEMP TABLE ' || tmp_account || ' as 
            //        SELECT 
            //        a.id    as gkh_account_id,
            //        r.id    as gkh_room_id, 
            //        r.ro_id as gkh_house_id, 

            //        a.acc_owner_id, 
    
            //        r.carea, 
            //        r.larea
    
            //        FROM REGOP_CALC_ACC_RO ar, REGOP_CALC_ACC acc, gkh_room r, regop_pers_acc a
            //        WHERE ar.account_id = acc.id
            //        and r.ro_id = ar.ro_id
            //        and r.id    = a.room_id
            //        and acc.account_owner_id = ' || GKH_CONTRAGENT_ID;

            //        if (gkh_house_id > 0) then
            //        sql := sql || ' and ar.ro_id = ' || gkh_house_id;
            //        end if;  

            //        sql := sql || ' GROUP BY r.ro_id, r.id, r.carea, r.larea, a.id';
     
            //        execute sql;

            //        execute 'create index ix_' || tmp_account || '_ids on ' || tmp_account || '(gkh_account_id, gkh_room_id, gkh_house_id)';
            //        execute 'analyze ' || tmp_account;
  
            //        error_text := 'ok';  
            //        error_code := 0;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_select_account(text, integer, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_select_account_wallet(
            //        IN tmp_account text,
            //        IN in_ris_contragent_id integer,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос лицевых счетов с кошельками в ris_account (САХАЛИН)                                   
            //    --=================================================================
            //    DECLARE
            //        sql text;

            //        gkh_contragent_id integer;
            //        gkh_house_id     integer;
            //    BEGIN
            //        in_ris_house_id := coalesce(in_ris_house_id, 0);
            //        in_ris_contragent_id := coalesce(in_ris_contragent_id, 0);

            //        GKH_CONTRAGENT_ID := ris.get_gkh_contragent_id(in_ris_contragent_id);
            //        gkh_house_id := ris.get_gkh_house_id(in_ris_house_id);

            //        PERFORM ris.drop_temp_table('tmp_select_account_room');
  
            //        -- выбрать список помещений
            //        sql := 'CREATE TEMP TABLE tmp_select_account_room as 
            //        SELECT 
            //        r.ro_id as gkh_house_id, r.id as room_id, r.carea, r.larea, ro.fias_address_id, r.croom_num, r.type, acc.account_owner_id
	           //     , acc.credit_org_id, acc.account_number
            //        FROM REGOP_CALC_ACC_RO ar, REGOP_CALC_ACC acc
	           //     , gkh_room r
		          //      join gkh_reality_object ro on r.ro_id = ro.id
            //        WHERE ar.account_id = acc.id
            //        and r.ro_id = ar.ro_id
            //        and acc.account_owner_id = ' || GKH_CONTRAGENT_ID;

            //        if (gkh_house_id > 0) then
            //        sql := sql || ' and ar.ro_id = ' || gkh_house_id;
            //        end if;  
     
            //        execute sql || ' GROUP BY r.ro_id, r.id, r.carea, r.larea, ro.fias_address_id, r.croom_num, r.type, acc.account_owner_id
	           //     , acc.credit_org_id, acc.account_number';

            //        create index ix_tmp_select_account_room_1 on tmp_select_account_room(room_id);
            //        analyze tmp_select_account_room;
  
            //        -- получить ЛС
            //        execute 'CREATE TEMP TABLE ' || tmp_account || ' as
            //        SELECT
            //        a.id as gkh_account_id,
            //        a.room_id,
            //        r.gkh_house_id,    
            //        a.acc_num as account_number,
            //        a.acc_owner_id,
            //        r.account_owner_id calc_acc_owner_id,
            //        r.credit_org_id,
            //        r.fias_address_id,
            //        r.croom_num room_num,
            //        r.type room_type,
            //        r.account_number calc_acc_number,
            //        bt_wallet.wallet_guid bt_wallet_guid,
            //        dt_wallet.wallet_guid dt_wallet_guid,
            //        r_wallet.wallet_guid r_wallet_guid,
            //        p_wallet.wallet_guid p_wallet_guid,
            //        ss_wallet.wallet_guid ss_wallet_guid,
            //        pwp_wallet.wallet_guid pwp_wallet_guid,
            //        af_wallet.wallet_guid af_wallet_guid,
            //        raa_wallet.wallet_guid raa_wallet_guid
     
            //        FROM tmp_select_account_room r,
	           //     regop_pers_acc a
	           //     join REGOP_WALLET BT_WALLET on bt_wallet.id = a.bt_wallet_id
	           //     join REGOP_WALLET DT_WALLET on dt_wallet.id = a.dt_wallet_id
	           //     join REGOP_WALLET R_WALLET on r_wallet.id = a.r_wallet_id
	           //     join REGOP_WALLET P_WALLET on p_wallet.id = a.p_wallet_id
	           //     join REGOP_WALLET ss_WALLET on ss_wallet.id = a.ss_wallet_id
	           //     join REGOP_WALLET pwp_WALLET on pwp_wallet.id = a.pwp_wallet_id
	           //     join REGOP_WALLET af_WALLET on af_wallet.id = a.af_wallet_id
	           //     join REGOP_WALLET raa_WALLET on raa_wallet.id = a.raa_wallet_id
            //        WHERE a.room_id = r.room_id';

            //        PERFORM ris.drop_temp_table('tmp_select_account_room');

            //        error_text := 'ok';  
            //        error_code := 0;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_select_account_wallet(text, integer, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_account(
            //        IN tmp_account_table text,
            //        IN in_ris_contragent_id integer,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос лицевых счетов в ris_account (САХАЛИН)
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 10.10.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE
            //        sql text;
            //    BEGIN
            //        in_ris_contragent_id := coalesce(in_ris_contragent_id, 0);

            //        PERFORM ris.drop_temp_table('tmp_account');

            //        execute 'CREATE TEMP TABLE tmp_account as
            //        SELECT
            //        a.id as gkh_account_id,

            //        30 as typeaccount,
            //        0  as livingpersonsnumber, 
            //        0  as heatedarea,
            //        false as closed,
            //        false as is_renter,
    
            //        a.open_date as begindate,
            //        t.carea * a.area_share  as totalsquare, 
            //        t.larea * a.area_share  as residentialsquare, 
            //        a.acc_num as accountnumber
     
            //        FROM regop_pers_acc a, ' || tmp_account_table || ' t
            //        WHERE a.id = t.gkh_account_id ';

            //        create index ix_tmp_account_1 on tmp_account (gkh_account_id);
            //        analyze tmp_account;
  
            //        -- обновить данные
            //        update public.ris_account ris set 
            //        (typeaccount, livingpersonsnumber, totalsquare, residentialsquare, heatedarea, 
            //            closed, accountnumber, begindate, is_renter,
            //            object_version, object_edit_date, operation) =  
            //        (t.typeaccount, t.livingpersonsnumber, t.totalsquare, t.residentialsquare, t.heatedarea, 
            //            t.closed, t.accountnumber, t.begindate, t.is_renter,
            //            ris.object_version + 1, now(), 1) 
            //        from tmp_account t 
            //            where ris.external_id = t.gkh_account_id 
            //            and ris.gi_contragent_id = in_ris_contragent_id;

            //        -- вставить данные 
            //        insert into public.ris_account (gi_contragent_id, external_id, 
            //            typeaccount, livingpersonsnumber, totalsquare, residentialsquare, heatedarea, 
            //            closed, accountnumber, begindate, is_renter,
            //            object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //        select in_ris_contragent_id, t.gkh_account_id, 
            //            t.typeaccount, t.livingpersonsnumber, t.totalsquare, t.residentialsquare, t.heatedarea, 
            //            t.closed, t.accountnumber, t.begindate, t.is_renter,
            //            0, now(), now(), 0, 'gkh'
            //        from tmp_account t
            //        where not exists (select 1 from public.ris_account ris where ris.external_id = t.gkh_account_id and ris.gi_contragent_id = in_ris_contragent_id);

            //        PERFORM ris.drop_temp_table('tmp_account');
  
            //        error_text := 'ok';  
            //        error_code := 0;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_account(text, integer, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_account(
            //        IN in_ris_contragent_id integer,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос лицевых счетов в ris_account (САХАЛИН)
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 04.10.2016                                       
            //    -- Дата изменения: 11.10.2016                                     
            //    --=================================================================
            //    BEGIN
            //        PERFORM ris.drop_temp_table('tmp_selected_account');

            //        PERFORM ris.ris_select_account('tmp_selected_account', in_ris_contragent_id, in_ris_house_id);
            //        PERFORM ris.ris_transfer_account('tmp_selected_account', in_ris_contragent_id); 
            //        PERFORM ris.ris_transfer_account_owner('tmp_selected_account', in_ris_contragent_id);
            //        PERFORM ris.ris_transfer_account_relation('tmp_selected_account', in_ris_contragent_id);
  
            //        PERFORM ris.drop_temp_table('tmp_selected_account');
  
            //        error_text := 'ok';  
            //        error_code := 0;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_account(integer, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_account_owner(
            //        in_temp_table_account text,
            //        in_ris_contragent_id integer)
            //        RETURNS text AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос абонентов в ЛС
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 04.10.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE
            //        dict_id integer;
            //    BEGIN
            //        PERFORM ris.drop_temp_table('tmp_account_owner');
            //        PERFORM ris.drop_temp_table('tmp_ind');

            //        -- получить ЛС
            //        execute 'CREATE TEMP TABLE tmp_account_owner as
            //        SELECT 
            //        t.gkh_account_id,
            //        t.acc_owner_id, 
            //        0 as ris_contragent_id,
            //        0 as ris_ind_id
            //        FROM ' || in_temp_table_account || ' t ';

            //        CREATE INDEX ix_tmp_account_owner_1 on tmp_account_owner (gkh_account_id, acc_owner_id);
            //        ANALYZE tmp_account_owner;
  
            //        -- сопоставить ЮЛ 
            //        UPDATE tmp_account_owner t SET ris_contragent_id = gi.id         
            //        FROM  gi_contragent gi, REGOP_LEGAL_ACC_OWN l
            //        WHERE gi.gkhid = l.contragent_id
            //        and l.id = t.acc_owner_id;

            //        dict_id := ris.get_dict_id('OwnerDocumentTypeDictionary');
  
            //        -- получить список физических лиц
            //        create temp table tmp_ind as
            //        select
            //        i.id as external_id, 
            //        i.surname,
            //        i.first_name as firstname,
            //        i.second_name as patronymic,
            //        (case when i.gender = 10 then 1 else 0 end) as sex,
            //        i.birth_date as dateofbirth,
            //        i.id_serial as idseries,
            //        i.id_num    as idnumber,
            //        i.birth_place as placebirth,
            //        i.date_document_issuance as idissuedate,

            //        gisd.gis_rec_guid as idtype_guid,
            //        gisd.gis_rec_id   as idtype_code 
    
            //        from tmp_account_owner t, REGOP_INDIVIDUAL_ACC_OWN i
            //        left outer join gi_integr_ref_dict gisd on gisd.gkh_rec_id = i.id_type
            //        where i.id = t.acc_owner_id;
  
            //        create index ix_tmp_ind_1 on tmp_ind(external_id);
            //        analyze tmp_ind;    

            //        -- обновить данные
            //        update public.ris_ind ris set 
            //        (surname, firstname, patronymic, sex, dateofbirth, 
            //            idtype_guid, idtype_code, 
            //            idseries, idnumber, idissuedate,
            //            placebirth,
            //            object_version, object_edit_date, operation) =  
            //        (t.surname, t.firstname, t.patronymic, t.sex, t.dateofbirth, 
            //            t.idtype_guid, t.idtype_code, 
            //            t.idseries, t.idnumber, t.idissuedate,
            //            t.placebirth,
            //            ris.object_version + 1, now(), 1) 
            //        from tmp_ind t 
            //            where ris.external_id = t.external_id 
            //                and ris.gi_contragent_id = in_ris_contragent_id;

            //        -- вставить данные 
            //        insert into public.ris_ind (gi_contragent_id, external_id, 
            //            surname, firstname, patronymic, sex, dateofbirth, 
            //            idtype_guid, idtype_code, 
            //            idseries, idnumber, idissuedate,
            //            placebirth,
            //            object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //        select in_ris_contragent_id, t.external_id, 
            //            t.surname, t.firstname, t.patronymic, t.sex, t.dateofbirth, 
            //            t.idtype_guid, t.idtype_code, 
            //            t.idseries, t.idnumber, t.idissuedate,
            //            t.placebirth,
            //            0, now(), now(), 0, 'gkh'
            //        from tmp_ind t
            //        where not exists (select 1 from public.ris_ind ris where ris.external_id = t.external_id and ris.gi_contragent_id = in_ris_contragent_id);

            //        -- проставить коды ФЛ
            //        UPDATE tmp_account_owner t SET ris_ind_id = ris.id FROM public.ris_ind ris WHERE ris.external_id = t.acc_owner_id;

            //        CREATE INDEX ix_tmp_account_owner_2 on tmp_account_owner (ris_ind_id, ris_contragent_id);
            //        ANALYZE tmp_account_owner;
  
            //        -- обновить ЛС
            //        UPDATE ris_account ris SET ownerind_id = t.ris_ind_id        FROM tmp_account_owner t where ris.external_id = t.gkh_account_id and t.ris_ind_id > 0;
            //        UPDATE ris_account ris SET ownerorg_id = t.ris_contragent_id FROM tmp_account_owner t where ris.external_id = t.gkh_account_id and t.ris_contragent_id > 0;
  
            //        PERFORM ris.drop_temp_table('tmp_account_owner');
            //        PERFORM ris.drop_temp_table('tmp_ind');
  
            //        return 'ok';
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_account_owner(text, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_account_relation(
            //        in_temp_table_account text,
            //        in_ris_contragent_id integer)
            //        RETURNS text AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос связей лицевых счетов в ris_account_relations
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 15.05.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE
            //        sql text;
            //    BEGIN
            //        PERFORM ris.drop_temp_table('tmp_account_relation');
  
            //        -- выбрать список помещений
            //        execute 'CREATE TEMP TABLE tmp_account_relation as 
            //        SELECT 
            //        t.gkh_account_id,
            //        t.gkh_room_id,
            //        t.gkh_house_id,

            //        0 as ris_account_id, 
            //        0 as residential_premise_id, 
            //        0 as ris_house_id
            //        FROM ' || in_temp_table_account || ' t, ris_account ris 
            //        WHERE t.gkh_account_id = ris.external_id ';

            //        create index ix_tmp_account_relation_gkh_ids on tmp_account_relation (gkh_room_id, gkh_house_id);
            //        analyze tmp_account_relation;
  
            //        UPDATE tmp_account_relation t SET residential_premise_id = ris.id FROM ris_residentialpremises ris WHERE t.gkh_room_id  = ris.external_id; 
            //        UPDATE tmp_account_relation t SET ris_house_id           = ris.id FROM ris_house               ris WHERE t.gkh_house_id = ris.external_id;

            //        CREATE INDEX ix_tmp_account_relation_ris_ids on tmp_account_relation (gkh_account_id, ris_house_id, residential_premise_id);
            //        ANALYZE tmp_account_relation;
  
            //        -- вставить данные
            //        insert into public.ris_account_relations (gi_contragent_id, 
            //        external_id, 
            //        account_id, house_id, residential_premise_id, nonresidential_premise_id, living_room_id,
            //        object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //        select in_ris_contragent_id, 
            //        t.gkh_account_id, 
            //        t.ris_account_id, t.ris_house_id, t.residential_premise_id, null, null,
            //        0, now(), now(), 0, 'ris'
            //        from tmp_account_relation t
            //        where not exists (select 1 from public.ris_account_relations ris where ris.external_id = t.gkh_account_id and ris.gi_contragent_id = in_ris_contragent_id)
            //        and t.ris_house_id > 0 and t.residential_premise_id > 0;

            //        PERFORM ris.drop_temp_table('tmp_account_relation');
  
            //        return 'ok';
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_account_relation(text, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_acknowledgment(
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$

            //    DECLARE
            //        p_period_id integer;
            //        p_gkh_contragent_id integer;
            //        sql text;
            //    BEGIN
            //        in_ris_house_id := coalesce(in_ris_house_id, 0);

            //        if in_report_month is null then
            //        error_text := 'Не передан отчетный период';  
            //        error_code := -1;
            //        return;
            //        end if;

            //        p_period_id := ris.get_regop_period_id(in_report_month);
            //        p_gkh_contragent_id := ris.get_gkh_contragent_id(in_ris_contragent_id);

            //        if p_period_id <= 0 then
            //        error_text := 'Не удалось определить отчетный период';  
            //        error_code := -1;
            //        return;
            //        end if;
  
            //        PERFORM ris.drop_temp_table('tmp_selected_account');
            //        PERFORM ris.drop_temp_table('tmp_acknowledgment');
  
            //        -- выбрать лицевые счета  
            //        PERFORM ris.ris_select_account('tmp_selected_account', in_ris_contragent_id, in_ris_house_id);
  
            //    CREATE TEMP TABLE tmp_acknowledgment AS
            //        SELECT distinct 
            //        ris.get_external_id(t.gkh_account_id, in_report_month) as external_id,  
            //        rn.order_id,
            //        rn.payment_document_id,
            //        rn.id as notify_id,
            //        2 as hstype,
            //        sum(rn.amount) as amount
            //        FROM tmp_selected_account t
            //        JOIN public.RIS_Payment_doc pd on pd.account_id = t.gkh_account_id
            //        JOIN public.ris_payment_info pi on pi.id = pd.payment_info_id
            //        JOIN public.RIS_NOTIFORDEREXECUT rn on rn.payment_doc_id = pd.id
            //        WHERE pi.gi_contragent_id = p_gkh_contragent_id
            //        GROUP BY 1,2,3,4,5;

            //        -- обновить данные
            //        update public.ris_acknowledgment ris set 
            //        ( orderid, pay_doc_id, notify_id, hstype, amount, 
            //            object_version, object_edit_date, operation) =  
            //        ( tt.order_id, tt.payment_document_id::bigint, 
            //            tt.notify_id,
            //            tt.hstype,
            //            tt.amount, 
            //            ris.object_version + 1, now(), 1 ) 
            //        from tmp_acknowledgment tt
            //            where ris.external_id = tt.external_id 
            //            and ris.gi_contragent_id = in_ris_contragent_id
            //            and ris.external_system_name = 'gkh';

            //        -- вставить данные 
            //        insert into public.ris_acknowledgment (gi_contragent_id, external_id, 
            //        orderid, pay_doc_id, notify_id, hstype, amount,
            //            object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //        select in_ris_contragent_id, tt.external_id, 
            //        tt.order_id, tt.payment_document_id::bigint, 
            //            tt.notify_id,
            //            tt.hstype,
            //            tt.amount,
            //        0, now(), now(), 0, 'gkh'
            //        from tmp_acknowledgment tt
            //        where not exists (select 1 from public.ris_acknowledgment ris where ris.external_id = tt.external_id and ris.gi_contragent_id = in_ris_contragent_id);

            //        PERFORM ris.drop_temp_table('tmp_selected_account');
            //        PERFORM ris.drop_temp_table('tmp_acknowledgment');

            //        error_text := 'ok';  
            //        error_code := 0;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_acknowledgment(integer, date, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_address_info(
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос площадей и количества проживающих из ЕПД
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 27.05.2016                                     
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE
            //        result record;
            //    BEGIN
            //        PERFORM ris.drop_temp_table('tmp_selected_account');
            //        PERFORM ris.ris_select_account('tmp_selected_account', in_ris_contragent_id, in_ris_house_id);
  
            //        select * into result from ris.ris_transfer_address_info('tmp_selected_account', in_ris_contragent_id, in_report_month); 

            //        PERFORM ris.drop_temp_table('tmp_selected_account');
  
            //        error_text := result.error_text;  
            //        error_code := result.error_code;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_address_info(integer, date, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_address_info(
            //        IN tmp_account_table text,
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос площадей и количества проживающих из ЕПД
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 27.05.2016                                     
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE
            //        p_report_id integer;
            //        sql text;
            //    BEGIN
            //        in_ris_house_id := coalesce(in_ris_house_id, 0);

            //        if in_report_month is null then
            //        error_text := 'Не передан отчетный период';  
            //        error_code := -1;
            //        return;
            //        end if;

            //        p_report_id := ris.get_regop_period_id(in_report_month);

            //        if p_report_id <= 0 then
            //        error_text := 'Не удалось определить отчетный период';  
            //        error_code := -1;
            //        return;
            //        end if;
  
            //        PERFORM ris.drop_temp_table('tmp_pay_doc');

            //        -- получить данные для ЕПД
            //        execute 'CREATE TEMP TABLE tmp_pay_doc as   
            //        select 
            //            ris.get_external_id(t.gkh_account_id, ' || quote_literal(in_report_month) || ') as pay_doc_id,
            //            t.livingpersonsnumber,
            //            t.residentialsquare,
            //            t.heatedarea,
            //            t.totalsquare
            //        FROM ' || tmp_account_table || ' t
            //        WHERE exists (SELECT 1 FROM REGOP_PERS_ACC_PERIOD_SUMM s where s.account_id = t.gkh_account_id AND s.period_id = ' || p_report_id || ')';

            //        create index ix_tmp_pay_doc_pay_doc_id on tmp_pay_doc (pay_doc_id);
            //        analyze tmp_pay_doc;

            //        -- обновить данные
            //        update public.ris_address_info ris set 
            //        (living_person_number, residential_square, heated_area, total_square, object_version, object_edit_date, operation) =  
            //        (tt.livingpersonsnumber, tt.residentialsquare, tt.heatedarea, tt.totalsquare, ris.object_version + 1, now(), 1 ) 
            //        from tmp_pay_doc tt
            //            where ris.external_id = tt.pay_doc_id 
            //            and ris.gi_contragent_id = in_ris_contragent_id
            //            and ris.external_system_name = 'gkh';

            //        -- вставить данные 
            //        insert into public.ris_address_info (gi_contragent_id, external_id, 
            //            living_person_number, residential_square, heated_area, total_square,
            //            object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //        select in_ris_contragent_id, tt.pay_doc_id, 
            //        tt.livingpersonsnumber, tt.residentialsquare, tt.heatedarea, tt.totalsquare,
            //        0, now(), now(), 0, 'gkh'
            //        from tmp_pay_doc tt
            //        where not exists (select 1 from public.ris_address_info ris where ris.external_id = tt.pay_doc_id and ris.gi_contragent_id = in_ris_contragent_id);

            //        PERFORM ris.drop_temp_table('tmp_pay_doc');
  
            //        error_text := 'ok';  
            //        error_code := 0;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_address_info(text, integer, date, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_capital_repair_charge(
            //        IN tmp_account_table text,
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос начислений по кап. ремонту
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 10.10.2016                                     
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE
            //        p_period_id integer;
            //        sql text;
            //    BEGIN
            //        in_ris_house_id := coalesce(in_ris_house_id, 0);

            //        if in_report_month is null then
            //        error_text := 'Не передан отчетный период';  
            //        error_code := -1;
            //        return;
            //        end if;

            //        p_period_id := ris.get_regop_period_id(in_report_month);

            //        if p_period_id <= 0 then
            //        error_text := 'Не удалось определить отчетный период';  
            //        error_code := -1;
            //        return;
            //        end if;
  
            //        PERFORM ris.drop_temp_table('tmp_charge');

            //        -- получить данные для ЕПД
            //        execute 'CREATE TEMP TABLE tmp_charge as   
            //        SELECT
            //        ch.id as external_id,
            //        t.gkh_account_id,  
            //        ris.get_external_id(t.gkh_account_id, ' || quote_literal(in_report_month) || ') as gkh_payment_doc_id,
            //        0 as ris_payment_doc_id,

            //        0.00 as contribution,
            //        0.00 as accounting_period_total,
            //        0.00 as money_recalculation,
            //        0.00 as money_discount,
    
            //        coalesce(ch.CHARGE_BASE_TARIFF, 0) + coalesce(ch.BALANCE_CHANGE, 0) + coalesce(ch.RECALC, 0) as charged_base_tariff,             -- Начислено взносов по минимальному тарифу всего
            //        coalesce(ch.CHARGE_TARIFF, 0) + coalesce(ch.CHARGE_BASE_TARIFF, 0) + coalesce(ch.RECALC_DECISION, 0) as charged_decision_tariff, --Начислено взносов по тарифу решения всего
            //        coalesce(ch.CHARGE_BASE_TARIFF, 0) - (coalesce(ch.tariff_payment, 0) + coalesce(ch.tariff_desicion_payment, 0) + coalesce(ch.penalty_payment, 0)) as saldo_change, --Изменение сальдо
            //        coalesce(ch.RECALC, 0) + coalesce(ch.RECALC_DECISION, 0) as recalc, --перерасчет


            //        ch.CHARGE_BASE_TARIFF as total_payable
            //        FROM
            //        ' || tmp_account_table || ' t,
            //        REGOP_PERS_ACC_PERIOD_SUMM ch
            //        where t.gkh_account_id = ch.account_id
            //        and ch.period_id = ' || p_period_id;

            //        create index ix_tmp_charge_1 on tmp_charge(gkh_payment_doc_id, gkh_account_id, external_id);
            //                analyze tmp_charge;

            //                UPDATE tmp_charge t set ris_payment_doc_id = ris.id FROM ris_payment_doc ris WHERE ris.external_id = t.gkh_payment_doc_id;
            //                UPDATE tmp_charge t set contribution = a.tariff     FROM regop_pers_acc  a WHERE a.id = t.gkh_account_id;

            //                --Всего начислено за расчетный период = Начислено взносов по минимальному тарифу всего + Начислено взносов по тарифу решения всего
            //        UPDATE tmp_charge t set accounting_period_total = t.charged_base_tariff + t.charged_decision_tariff;
            //                --Перерасчеты, корректировки = Перерасчет + Изменение сальдо
            //        UPDATE tmp_charge t set money_recalculation = t.saldo_change + t.recalc;

            //                --обновить данные
            //                UPDATE public.ris_capital_repair_charge ris set
            //                    (contribution, accounting_period_total, money_recalculation, money_discount, total_payable,
            //                    object_version, object_edit_date, operation) =  
            //        (tt.contribution, tt.accounting_period_total, tt.money_recalculation, tt.money_discount, tt.total_payable, 
            //            ris.object_version + 1, now(), 1 ) 
            //        from tmp_charge tt
            //            where ris.external_id = tt.external_id
            //            and ris.gi_contragent_id = in_ris_contragent_id
            //            and ris.external_system_name = 'gkh';

            //        -- вставить данные
            //        insert into public.ris_capital_repair_charge(
            //            gi_contragent_id,
            //            external_id,
            //            payment_doc_id,
            //            contribution, accounting_period_total, money_recalculation, money_discount, total_payable,
            //            object_version, object_create_date, object_edit_date, operation, external_system_name)
            //        select
            //        in_ris_contragent_id,
            //        tt.external_id,
            //        tt.ris_payment_doc_id, 
            //        tt.contribution, tt.accounting_period_total, tt.money_recalculation, tt.money_discount, tt.total_payable,
            //        0, now(), now(), 0, 'gkh'
            //        from tmp_charge tt
            //        where not exists(select 1 from public.ris_capital_repair_charge ris where ris.external_id = tt.external_id and ris.gi_contragent_id = in_ris_contragent_id);

            //            PERFORM ris.drop_temp_table('tmp_charge');

            //            error_text := 'ok';  
            //        error_code := 0;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //            ALTER FUNCTION ris.ris_transfer_capital_repair_charge(text, integer, date, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_capital_repair_charge(
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос начислений по кап. ремонту
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 07.10.2016                                     
            //    -- Дата изменения: 10.10.2016                                     
            //    --=================================================================
            //    DECLARE
            //        result record;
            //    BEGIN
            //        PERFORM ris.drop_temp_table('tmp_selected_account');
            //        PERFORM ris.ris_select_account('tmp_selected_account', in_ris_contragent_id, in_ris_house_id);
  
            //        select * into result from ris.ris_transfer_capital_repair_charge('tmp_selected_account', in_ris_contragent_id, in_report_month); 

            //        PERFORM ris.drop_temp_table('tmp_selected_account');
  
            //        error_text := result.error_text;  
            //        error_code := result.error_code;
  
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_capital_repair_charge(integer, date, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_capital_repair_debt(
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос данных расчетных счетов, по которым выставлены ЕПД
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 07.10.2016                                       
            //    -- Дата изменения: 10.10.2016                                        
            //    --=================================================================
            //    DECLARE
            //        result record;
            //    BEGIN
            //        PERFORM ris.drop_temp_table('tmp_selected_account');
            //        PERFORM ris.ris_select_account('tmp_selected_account', in_ris_contragent_id, in_ris_house_id);
  
            //        select * into result from ris.ris_transfer_capital_repair_debt('tmp_selected_account', in_ris_contragent_id, in_report_month); 

            //        PERFORM ris.drop_temp_table('tmp_selected_account');
  
            //        error_text := result.error_text;  
            //        error_code := result.error_code;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_capital_repair_debt(integer, date, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_notiforderexecut(
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос извещений о принятии к исполнению распоряжения                                 
            //    --=================================================================
            //    DECLARE
            //        gkh_contragent_id integer;
            //        gkh_house_id     integer;
            //    BEGIN

            //    if in_report_month is null then
            //        error_text := 'Не передан отчетный период';  
            //        error_code := -1;
            //        return;
            //        end if;

            //    in_ris_house_id := coalesce(in_ris_house_id, 0);
            //    in_ris_contragent_id := coalesce(in_ris_contragent_id, 0);

            //    GKH_CONTRAGENT_ID := ris.get_gkh_contragent_id(in_ris_contragent_id);
            //    gkh_house_id := ris.get_gkh_house_id(in_ris_house_id);

            //    PERFORM ris.drop_temp_table('tmp_acc');
            //    PERFORM ris.drop_temp_table('cur_month_transfer');
            //    PERFORM ris.drop_temp_table('tmp_allowed_transfer');
            //    PERFORM ris.drop_temp_table('res_tmp');

            //    --выбираем ЛС
            //    perform ris.ris_select_account_wallet('tmp_acc', in_ris_contragent_id, in_ris_house_id);

            //    --выбираем оплаты за указанный месяц
            //    create temp table cur_month_transfer as
            //    select transfer.* 
            //    from REGOP_TRANSFER transfer
	           //     join REGOP_MONEY_OPERATION m_op on transfer.OP_ID = m_op.id and m_op.CANCELED_OP_ID is null
	           //     left join REGOP_MONEY_OPERATION rmo on transfer.OP_ID = rmo.CANCELED_OP_ID
            //    where extract(year from transfer.OPERATION_DATE) = extract(year from in_report_month) 
	           //     and extract(month from transfer.OPERATION_DATE) = extract(month from in_report_month) 
	           //     and not IS_INDIRECT and rmo.id is null;

            //    create index ix_tmp_cur_month_transfer_target_guid on cur_month_transfer(target_guid);
            //    create index ix_tmp_cur_month_transfer_source_guid on cur_month_transfer(source_guid);
            //    analyze cur_month_transfer;

            //    --выбираем оплаты, связанные с выбранными ЛС
            //    --связка с transfer'ом через target_guid
            //    --BT_WALLET_ID
            //    create temp table tmp_allowed_transfer as
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.bt_wallet_guid;

            //    --DT_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.dt_wallet_guid;

            //    --R_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.r_wallet_guid;

            //    --P_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.p_wallet_guid;

            //    --SS_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.ss_wallet_guid;

            //    --PWP_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.pwp_wallet_guid;

            //    --AF_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.af_wallet_guid;

            //    --RAA_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.raa_wallet_guid;

            //    --связка с transfer'ом через source_guid
            //    --BT_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.source_guid = acc.bt_wallet_guid and not transfer.IS_COPY_FOR_SOURCE and transfer.reason ilike '%возврат%';

            //    --DT_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.dt_wallet_guid;

            //    --R_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.r_wallet_guid;

            //    --P_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.p_wallet_guid;

            //    --SS_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.ss_wallet_guid;

            //    --PWP_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.pwp_wallet_guid;

            //    --AF_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.af_wallet_guid;

            //    --RAA_WALLET_ID
            //    insert into tmp_allowed_transfer
            //    select transfer.id external_id, transfer.id order_id, transfer.PAYMENT_DATE order_date, transfer.amount, transfer.reason payment_purpose
	           //     , acc.acc_owner_id, acc.gkh_account_id, acc.fias_address_id, acc.room_num, acc.room_type, acc.account_number, acc.calc_acc_owner_id
	           //     , acc.credit_org_id, acc.calc_acc_number
            //    from cur_month_transfer transfer
	           //     join tmp_acc acc on transfer.target_guid = acc.raa_wallet_guid;

            //    --формируем итоговую выборку
            //    create temp table res_tmp as
            //    select distinct transfer.external_id, acc_owner.name supplier_name, transfer.order_id, transfer.order_date, transfer.amount, transfer.payment_purpose
	           //     , ris_pay_doc.external_id payment_doc_id, ris_pay_doc.payment_doc_num payment_document_number, faddr.house_guid fias_house_guid
	           //     , extract(year from transfer.order_date) ""year"", extract(month from transfer.order_date) ""month"", transfer.account_number
            //        , case when transfer.room_type = 10 then transfer.room_num end apartment, case when transfer.room_type = 20 then transfer.room_num end non_living_apartment
            //        , contragent.inn consumer_inn, regop_contragent.inn, regop_contragent.kpp RECIPIENT_LEGAL_KPP, regop_contragent.name RECIPIENT_LEGAL_NAME, regop_contragent.inn recipient_inn
            //        , acc_owner_ind.first_name consumer_first_name, acc_owner_ind.surname consumer_surname, acc_owner_ind.second_name consumer_patronymic
            //        , regop_contragent.kpp recipient_kpp, credit_org.name bank_name, regop_contragent.name recipient_name, credit_org.bik recipient_bank_bik
            //        , credit_org.corr_account recipient_bank_corracc, transfer.calc_acc_number recipient_account
            //    from tmp_allowed_transfer transfer

            //        join REGOP_PERS_ACC_OWNER acc_owner on transfer.acc_owner_id = acc_owner.id

            //        left
            //    join ris_payment_doc ris_pay_doc on ris.get_external_id(transfer.gkh_account_id, in_report_month) = ris_pay_doc.external_id and ris_pay_doc.operation != 2 and ris_pay_doc.external_system_name = 'gkh'

            //        join b4_fias_address faddr on transfer.fias_address_id = faddr.id

            //        left join REGOP_INDIVIDUAL_ACC_OWN acc_owner_ind on acc_owner_ind.id = transfer.acc_owner_id

            //        left join REGOP_LEGAL_ACC_OWN acc_owner_jur

            //            join GKH_CONTRAGENT contragent on contragent.id = acc_owner_jur.CONTRAGENT_ID

            //        on acc_owner_jur.id = transfer.acc_owner_id

            //        join GKH_CONTRAGENT regop_contragent on transfer.calc_acc_owner_id = regop_contragent.id

            //        join OVRHL_CREDIT_ORG credit_org on transfer.CREDIT_ORG_ID = credit_org.id;

            //                create index ix_res_tmp_external_id on res_tmp(external_id);
            //                analyze res_tmp;

            //                --обновить данные
            //                update public.RIS_NOTIFORDEREXECUT ris set
            //                (external_id, supplier_name, order_id, order_date, amount, payment_purpose, PAYMENT_DOCUMENT_ID, payment_document_number, ""year"", ""month"", apartment
            
            //                    , non_living_apartment, account_number, fias_house_guid, consumer_first_name, consumer_surname, consumer_patronymic, consumer_inn
            
            //                    , inn, recipient_legal_kpp, recipient_legal_name, recipient_inn, recipient_kpp, bank_name, recipient_name, recipient_bank_bik
            
            //                    , recipient_account, recipient_bank_corracc, object_version, object_edit_date, operation) =  
            //    (tt.external_id, tt.supplier_name, tt.order_id, tt.order_date, tt.amount, tt.payment_purpose, tt.payment_doc_id
            //    , tt.payment_document_number, tt.""year"", tt.""month"", tt.apartment, tt.non_living_apartment, tt.account_number
            //    , tt.fias_house_guid, tt.consumer_first_name, tt.consumer_surname, tt.consumer_patronymic, tt.consumer_inn
            //    , tt.inn, tt.recipient_legal_kpp, tt.recipient_legal_name, tt.recipient_inn, tt.recipient_kpp, tt.bank_name, tt.recipient_name
            //    , tt.recipient_bank_bik, tt.recipient_account, tt.recipient_bank_corracc
            //    , ris.object_version + 1, now(), 1 ) 
            //    from res_tmp tt
            //    where ris.external_id = tt.external_id
            //    and ris.gi_contragent_id = in_ris_contragent_id
            //    and ris.external_system_name = 'gkh';

            //    -- вставить данные
            //    insert into public.RIS_NOTIFORDEREXECUT(gi_contragent_id, external_id, supplier_name, order_id, order_date, amount, payment_purpose, PAYMENT_DOCUMENT_ID
            //    , payment_document_number, ""year"", ""month"", apartment, non_living_apartment, account_number
            //    , fias_house_guid, consumer_first_name, consumer_surname, consumer_patronymic, consumer_inn
            //    , inn, recipient_legal_kpp, recipient_legal_name, recipient_inn, recipient_kpp, bank_name, recipient_name
            //    , recipient_bank_bik, recipient_account, recipient_bank_corracc,
            //    object_version, object_create_date, object_edit_date, operation, external_system_name)
            //    select in_ris_contragent_id, tt.external_id, tt.supplier_name, tt.order_id, tt.order_date, tt.amount, tt.payment_purpose, tt.payment_doc_id
            //    , tt.payment_document_number, tt.""year"", tt.""month"", tt.apartment, tt.non_living_apartment, tt.account_number
            //    , tt.fias_house_guid, tt.consumer_first_name, tt.consumer_surname, tt.consumer_patronymic, tt.consumer_inn
            //    , tt.inn, tt.recipient_legal_kpp, tt.recipient_legal_name, tt.recipient_inn, tt.recipient_kpp, tt.bank_name, tt.recipient_name
            //    , tt.recipient_bank_bik, tt.recipient_account, tt.recipient_bank_corracc
            //    , 0, now(), now(), 0, 'gkh'
            //    from res_tmp tt
            //    where not exists(select 1 from public.RIS_NOTIFORDEREXECUT ris where ris.external_id = tt.external_id and ris.gi_contragent_id = in_ris_contragent_id);

            //            error_text := 'ok';  
            //    error_code := 0;
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //            ALTER FUNCTION ris.ris_transfer_notiforderexecut(integer, date, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_payment_doc(
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос данных расчетных счетов, по которым выставлены ЕПД
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 07.10.2016                                       
            //    -- Дата изменения: 10.10.2016                                        
            //    --=================================================================
            //    DECLARE
            //        result record;
            //    BEGIN
            //        PERFORM ris.drop_temp_table('tmp_selected_account');
            //        PERFORM ris.ris_select_account('tmp_selected_account', in_ris_contragent_id, in_ris_house_id);
  
            //        select * into result from ris.ris_transfer_payment_doc('tmp_selected_account', in_ris_contragent_id, in_report_month); 

            //        PERFORM ris.drop_temp_table('tmp_selected_account');
  
            //        error_text := result.error_text;  
            //        error_code := result.error_code;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_payment_doc(integer, date, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_payment_doc(
            //        IN tmp_account_table text,
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос данных расчетных счетов, по которым выставлены ЕПД
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 07.10.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE
            //        p_period_id integer;
            //        p_gkh_contragent_id integer;
            //        sql text;
            //    BEGIN
            //        in_ris_house_id := coalesce(in_ris_house_id, 0);

            //        if in_report_month is null then
            //        error_text := 'Не передан отчетный период';  
            //        error_code := -1;
            //        return;
            //        end if;

            //        p_period_id := ris.get_regop_period_id(in_report_month);
            //        p_gkh_contragent_id := ris.get_gkh_contragent_id(in_ris_contragent_id);

            //        if p_period_id <= 0 then
            //        error_text := 'Не удалось определить отчетный период';  
            //        error_code := -1;
            //        return;
            //        end if;
  
            //        PERFORM ris.drop_temp_table('tmp_payment_doc');
            //        PERFORM ris.drop_temp_table('tmp_account_rs');

            //        -- связь между ЛС и расчетными счетами
            //        execute 'CREATE TEMP TABLE tmp_account_rs AS
            //        SELECT distinct 
            //        t.gkh_account_id,
            //        ris.get_external_id(t.gkh_account_id, ' || quote_literal(in_report_month) || ') as external_id,  
            //        rs.id as gkh_bank_account_id
            //        FROM ' || tmp_account_table || ' t, REGOP_CALC_ACC_RO ar, REGOP_CALC_ACC rs
            //        WHERE t.gkh_house_id = ar.ro_id
            //        and ar.account_id = rs.id
            //        and rs.account_owner_id = ' || p_gkh_contragent_id;

            //        create index ix_tmp_account_rs_1 on tmp_account_rs (external_id, gkh_bank_account_id);
            //        analyze tmp_account_rs;

            //        -- данные ЕПД
            //        create temp table tmp_payment_doc AS
            //        SELECT 
            //        a.gkh_account_id, a.external_id, 
            //        0 as ris_account_id, 
            //        i.id as ris_address_info_id, 
            //        rs.id as ris_payment_info_id, 

            //        0.00 as total_piecemeal_sum,
            //        0.00 as advance_blling_period,
            //        0.00 as debt_previous_periods, 
    
            //        10   as state,  -- выставлен на оплату
            //        extract(year from in_report_month)::integer as periodyear,
            //        extract(month from in_report_month)::integer as periodmonth
    
            //        FROM tmp_account_rs a, ris_address_info i, ris_payment_info rs
            //        WHERE a.external_id = i.external_id
            //        and rs.external_id = a.gkh_bank_account_id 
            //        and i.gi_contragent_id = in_ris_contragent_id
            //        and rs.gi_contragent_id = in_ris_contragent_id;

            //        UPDATE tmp_payment_doc t SET debt_previous_periods = s.saldo_in
            //        FROM REGOP_PERS_ACC_PERIOD_SUMM s
            //        WHERE t.gkh_account_id = s.account_id 
            //        and s.period_id = p_period_id;

            //        -- получить назначенный код ЛС
            //        UPDATE tmp_payment_doc t SET ris_account_id = ris.id
            //        FROM ris_account ris
            //        WHERE t.gkh_account_id = ris.external_id 
            //        and ris.gi_contragent_id = in_ris_contragent_id;

            //        create index ix_tmp_payment_doc_1 on tmp_payment_doc (external_id);
            //        analyze tmp_payment_doc;

            //        -- обновить данные
            //        update public.ris_payment_doc ris set 
            //        ( account_id, payment_info_id, address_info_id, 
            //            total_piecemeal_sum,
            //            advance_blling_period,
            //            debt_previous_periods, 
            //            state, periodyear, periodmonth, 
            //            object_version, object_edit_date, operation) =  
            //        (tt.ris_account_id, tt.ris_payment_info_id, tt.ris_address_info_id, 
            //            tt.total_piecemeal_sum,
            //            tt.advance_blling_period,
            //            tt.debt_previous_periods, 
            //            tt.state, tt.periodyear, tt.periodmonth, 
            //            ris.object_version + 1, now(), 1 ) 
            //        from tmp_payment_doc tt
            //            where ris.external_id = tt.external_id 
            //            and ris.gi_contragent_id = in_ris_contragent_id
            //            and ris.external_system_name = 'gkh';

            //        -- вставить данные 
            //        insert into public.ris_payment_doc (gi_contragent_id, external_id, 
            //            account_id, payment_info_id, address_info_id, 
            //            total_piecemeal_sum,
            //            advance_blling_period,
            //            debt_previous_periods, 
            //            state, periodyear, periodmonth,
            //            object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //        select in_ris_contragent_id, tt.external_id, 
            //        tt.ris_account_id, tt.ris_payment_info_id, tt.ris_address_info_id, 
            //        tt.total_piecemeal_sum,
            //        tt.advance_blling_period,
            //        tt.debt_previous_periods, 
            //        tt.state, tt.periodyear, tt.periodmonth,
            //        0, now(), now(), 0, 'gkh'
            //        from tmp_payment_doc tt
            //        where not exists (select 1 from public.ris_payment_doc ris where ris.external_id = tt.external_id and ris.gi_contragent_id = in_ris_contragent_id);

            //        PERFORM ris.drop_temp_table('tmp_payment_doc');
            //        PERFORM ris.drop_temp_table('tmp_account_rs');
  
            //        error_text := 'ok';  
            //        error_code := 0;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_payment_doc(text, integer, date, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_payment_info(
            //        IN tmp_account_table text,
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос данных расчетных счетов, по которым выставлены ЕПД
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 10.10.2016                                  
            //    -- Дата изменения:                              
            //    --=================================================================
            //    DECLARE
            //        p_report_id integer;
            //        sql text;
            //        p_gkh_contragent_id integer;
            //    BEGIN
            //        in_ris_house_id := coalesce(in_ris_house_id, 0);

            //        if in_report_month is null then
            //        error_text := 'Не передан отчетный период';  
            //        error_code := -1;
            //        return;
            //        end if;

            //        p_report_id := ris.get_regop_period_id(in_report_month);

            //        if p_report_id <= 0 then
            //        error_text := 'Не удалось определить отчетный период';  
            //        error_code := -1;
            //        return;
            //        end if;

            //        p_gkh_contragent_id := ris.get_gkh_contragent_id(in_ris_contragent_id);
  
            //        PERFORM ris.drop_temp_table('tmp_bank_account');
            //        PERFORM ris.drop_temp_table('tmp_payment_info');

            //        -- выбрать р/с
            //        execute 'CREATE TEMP TABLE tmp_bank_oper_account AS
            //        SELECT rs.id, rs.account_owner_id, rs.credit_org_id, rs.account_number
            //        FROM ' || tmp_account_table || ' t, REGOP_CALC_ACC_RO ar, REGOP_CALC_ACC rs
            //        WHERE t.gkh_house_id = ar.ro_id
            //        and ar.account_id = rs.id
            //        and rs.account_owner_id = ' || p_gkh_contragent_id  || '
            //        GROUP BY rs.id, rs.account_owner_id, rs.credit_org_id, rs.account_number';

            //        -- данные для РИС
            //        CREATE TEMP TABLE tmp_payment_info AS
            //        SELECT 
            //        rs.id as gkh_bank_account_id, 
            //        bank.bik as bank_bik,
            //        bank.corr_account as correspondent_bank_account,
            //        bank.name as bank_name,
            //        COALESCE(rs.account_number, '') as operating_account_number,
            //        c.inn as recipient_inn,
            //        c.kpp as recipient_kpp,
            //        c.name as recipient
            //        from tmp_bank_oper_account rs, gkh_contragent c, ovrhl_credit_org bank
            //        where rs.account_owner_id = c.id
            //        and rs.credit_org_id = bank.id;

            //        create index ix_tmp_payment_info_1 on tmp_payment_info (gkh_bank_account_id);
            //        analyze tmp_payment_info;

            //        -- обновить данные
            //        UPDATE public.ris_payment_info ris SET 
            //        (recipient, recipient_kpp, recipient_inn,
            //            bank_bik, bank_name, operating_account_number, correspondent_bank_account,
            //            object_version, object_edit_date, operation) =  
            //        (tt.recipient, tt.recipient_kpp, tt.recipient_inn,
            //            tt.bank_bik, tt.bank_name, tt.operating_account_number, tt.correspondent_bank_account,
            //            ris.object_version + 1, now(), 1 ) 
            //        FROM tmp_payment_info tt
            //        WHERE 
            //                ris.external_id = tt.gkh_bank_account_id 
            //            and ris.gi_contragent_id = in_ris_contragent_id
            //            and ris.external_system_name = 'gkh';

            //        -- вставить данные 
            //        INSERT INTO public.ris_payment_info (gi_contragent_id, external_id, 
            //            recipient, recipient_kpp, recipient_inn,
            //            bank_bik, bank_name, operating_account_number, correspondent_bank_account,
            //            object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //        SELECT in_ris_contragent_id, tt.gkh_bank_account_id, 
            //        tt.recipient, tt.recipient_kpp, tt.recipient_inn,
            //        tt.bank_bik, tt.bank_name, tt.operating_account_number, tt.correspondent_bank_account,
            //        0, now(), now(), 0, 'gkh'
            //        FROM tmp_payment_info tt
            //        WHERE 
            //        NOT EXISTS (SELECT 1 FROM public.ris_payment_info ris WHERE 
            //            ris.external_id = tt.gkh_bank_account_id and ris.gi_contragent_id = in_ris_contragent_id);

            //        PERFORM ris.drop_temp_table('tmp_bank_account');
            //        PERFORM ris.drop_temp_table('tmp_payment_info');
  
            //        error_text := 'ok';  
            //        error_code := 0;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_payment_info(text, integer, date, integer)
            //        OWNER TO bars;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_payment_info(
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //        RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос данных расчетных счетов, по которым выставлены ЕПД
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 07.10.2016                                  
            //    -- Дата изменения: 10.10.2016 - передача временной таблицы в качестве параметра                                   
            //    --=================================================================
            //    DECLARE
            //        result record;
            //    BEGIN
            //        PERFORM ris.drop_temp_table('tmp_selected_account');
            //        PERFORM ris.ris_select_account('tmp_selected_account', in_ris_contragent_id, in_ris_house_id);
  
            //        select * into result from ris.ris_transfer_payment_info('tmp_selected_account', in_ris_contragent_id, in_report_month); 

            //        PERFORM ris.drop_temp_table('tmp_selected_account');
  
            //        error_text := result.error_text;  
            //        error_code := result.error_code;
    
            //        return;
            //    END;
            //    $BODY$
            //        LANGUAGE plpgsql VOLATILE
            //        COST 100;
            //    ALTER FUNCTION ris.ris_transfer_payment_info(integer, date, integer)
            //        OWNER TO bars;
            //");
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
        }
    }
}