namespace Bars.GisIntegration.RegOp.Migrations.Version_2016100601
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016100601")]
    [MigrationDependsOn(typeof(Version_2016100600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.ExecuteNonQuery(@"
            //    CREATE SCHEMA IF NOT EXISTS ris;

            //    CREATE OR REPLACE FUNCTION ris.drop_temp_table(temp_table_name text)
            //      RETURNS text AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: удаление временной таблицы
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 14.05.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE
            //      house text;
            //    BEGIN
            //      execute 'drop table ' || temp_table_name;
            //      return 'ok';
  
            //      EXCEPTION
            //        WHEN undefined_table THEN
            //        BEGIN
               
            //        END;
            //      return 'ok';
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;


            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_account_owner(in_temp_table_account text, in in_ris_contragent_id integer)
            //      RETURNS text AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос абонентов в ЛС
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 04.10.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE
            //      dict_id integer;
            //    BEGIN
            //      PERFORM ris.drop_temp_table('tmp_account_owner');
            //      PERFORM ris.drop_temp_table('tmp_ind');

            //      -- получить ЛС
            //      execute 'CREATE TEMP TABLE tmp_account_owner as
            //      SELECT 
            //        a.id as gkh_account_id,
            //        a.acc_owner_id, 
            //        0 as ris_contragent_id,
            //        0 as ris_ind_id
            //      FROM regop_pers_acc a, ' || in_temp_table_account || ' r
            //      WHERE a.id = r.gkh_account_id';

            //      -- сопоставить ЮЛ 
            //      UPDATE tmp_account_owner t SET ris_contragent_id = gi.id         
            //      FROM  gi_contragent gi, REGOP_LEGAL_ACC_OWN l
            //      WHERE gi.gkhid = l.contragent_id
            //        and l.id = t.acc_owner_id;

            //      dict_id := ris.get_dict_id('OwnerDocumentTypeDictionary');
  
            //      -- получить список физических лиц
            //      create temp table tmp_ind as
            //      select
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
    
            //      from tmp_account_owner t, REGOP_INDIVIDUAL_ACC_OWN i
            //        left outer join gi_integr_ref_dict gisd on gisd.gkh_rec_id = i.id_type
            //      where i.id = t.acc_owner_id;
  
            //      create index ix_tmp_ind_1 on tmp_ind(external_id);
            //      analyze tmp_ind;    

            //      -- обновить данные
            //      update public.ris_ind ris set 
            //        (surname, firstname, patronymic, sex, dateofbirth, 
            //          idtype_guid, idtype_code, 
            //          idseries, idnumber, idissuedate,
            //          placebirth,
            //          object_version, object_edit_date, operation) =  
            //        (t.surname, t.firstname, t.patronymic, t.sex, t.dateofbirth, 
            //          t.idtype_guid, t.idtype_code, 
            //          t.idseries, t.idnumber, t.idissuedate,
            //          t.placebirth,
            //          ris.object_version + 1, now(), 1) 
            //        from tmp_ind t 
            //          where ris.external_id = t.external_id 
            //             and ris.gi_contragent_id = in_ris_contragent_id;

            //      -- вставить данные 
            //      insert into public.ris_ind (gi_contragent_id, external_id, 
            //          surname, firstname, patronymic, sex, dateofbirth, 
            //          idtype_guid, idtype_code, 
            //          idseries, idnumber, idissuedate,
            //          placebirth,
            //          object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //        select in_ris_contragent_id, t.external_id, 
            //          t.surname, t.firstname, t.patronymic, t.sex, t.dateofbirth, 
            //          t.idtype_guid, t.idtype_code, 
            //          t.idseries, t.idnumber, t.idissuedate,
            //          t.placebirth,
            //          0, now(), now(), 0, 'gkh'
            //        from tmp_ind t
            //        where not exists (select 1 from public.ris_ind ris where ris.external_id = t.external_id and ris.gi_contragent_id = in_ris_contragent_id);

            //      -- проставить коды ФЛ
            //      UPDATE tmp_account_owner t SET ris_ind_id = ris.id FROM public.ris_ind ris WHERE ris.external_id = t.acc_owner_id;

            //      -- обновить ЛС
            //      UPDATE ris_account ris SET ownerind_id = t.ris_ind_id        FROM tmp_account_owner t where ris.external_id = t.gkh_account_id and t.ris_ind_id > 0;
            //      UPDATE ris_account ris SET ownerorg_id = t.ris_contragent_id FROM tmp_account_owner t where ris.external_id = t.gkh_account_id and t.ris_contragent_id > 0;
  
            //      PERFORM ris.drop_temp_table('tmp_account_owner');
            //      PERFORM ris.drop_temp_table('tmp_ind');
  
            //      return 'ok';
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;




            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_account_relation(in_temp_table_account text, in_ris_contragent_id integer)
            //      RETURNS text AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос связей лицевых счетов в ris_account_relations
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 15.05.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE
            //      sql text;
            //    BEGIN
            //      PERFORM ris.drop_temp_table('tmp_account_relation');
  
            //      -- выбрать список помещений
            //      execute 'CREATE TEMP TABLE tmp_account_relation as 
            //      SELECT 
            //        a.id      as gkh_account_id,
            //        a.room_id as gkh_room_id,
            //        r.ro_id   as gkh_house_id,

            //        0 as ris_account_id, 
            //        0 as residential_premise_id, 
            //        0 as ris_house_id
            //      FROM regop_pers_acc a, gkh_room r, ' || in_temp_table_account || ' t 
            //      WHERE a.id = t.gkh_account_id
            //        and a.room_id = r.id ';

            //      create index ix_tmp_account_relation_1 on tmp_account_relation (gkh_account_id, gkh_room_id, gkh_house_id);
            //      analyze tmp_account_relation;
  
            //      -- получить назначенные коды помещений
            //      UPDATE tmp_account_relation t SET residential_premise_id    = ris.id FROM ris_residentialpremises    ris WHERE t.gkh_room_id  = ris.external_id; 
            //      -- получить назначенные коды домов
            //      UPDATE tmp_account_relation t SET ris_house_id             = ris.id FROM ris_house                   ris WHERE t.gkh_house_id = ris.external_id;
            //      -- получить назначенный код ЛС 
            //      UPDATE tmp_account_relation t SET ris_account_id           = ris.id FROM ris_account                 ris WHERE t.gkh_account_id = ris.external_id;
  
            //      -- вставить данные
            //      insert into public.ris_account_relations (gi_contragent_id, 
            //        external_id, 
            //        account_id, house_id, residential_premise_id, nonresidential_premise_id, living_room_id,
            //        object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //      select in_ris_contragent_id, 
            //        t.gkh_account_id, 
            //        t.ris_account_id, t.ris_house_id, t.residential_premise_id, null, null,
            //        0, now(), now(), 0, 'ris'
            //      from tmp_account_relation t
            //      where not exists (select 1 from public.ris_account_relations ris where ris.external_id = t.gkh_account_id and ris.gi_contragent_id = in_ris_contragent_id)
            //        and t.ris_house_id > 0 and t.residential_premise_id > 0;

            //      PERFORM ris.drop_temp_table('tmp_account_relation');
  
            //      return 'ok';
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;


            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_account(IN in_ris_contragent_id integer, IN in_ris_house_id integer DEFAULT 0, OUT error_code integer, OUT error_text text)
            //      RETURNS record AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Перенос лицевых счетов в ris_account (САХАЛИН)
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 04.10.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE
            //      sql text;

            //      gkh_contragent_id integer;
            //      gkh_house_id     integer;
            //    BEGIN
            //      in_ris_house_id := coalesce(in_ris_house_id, 0);
            //      in_ris_contragent_id := coalesce(in_ris_contragent_id, 0);

            //      GKH_CONTRAGENT_ID := ris.get_gkh_contragent_id(in_ris_contragent_id);
            //      gkh_house_id := ris.get_gkh_house_id(in_ris_house_id);

            //      PERFORM ris.drop_temp_table('tmp_room');
            //      PERFORM ris.drop_temp_table('tmp_account');
  
            //      -- выбрать список помещений
            //      sql := 'CREATE TEMP TABLE tmp_room as 
            //      SELECT 
            //        r.id as room_id, r.carea, r.larea
            //      FROM REGOP_CALC_ACC_RO ar, REGOP_CALC_ACC acc, gkh_room r
            //      WHERE ar.account_id = acc.id
            //        and r.ro_id = ar.ro_id
            //        and acc.account_owner_id = ' || GKH_CONTRAGENT_ID;

            //      if (gkh_house_id > 0) then
            //        sql := sql || ' and ar.ro_id = ' || gkh_house_id;
            //      end if;  
     
            //      execute sql || ' GROUP BY r.id, r.carea, r.larea';
  
            //      -- получить ЛС
            //      CREATE TEMP TABLE tmp_account as
            //      SELECT
            //        a.id as gkh_account_id,
    
            //        30 as typeaccount,
            //        0  as livingpersonsnumber, 
            //        0  as heatedarea,
            //        false as closed,
            //        false as is_renter,
    
            //        a.open_date as begindate,
            //        r.carea * a.area_share  as totalsquare, 
            //        r.larea * a.area_share  as residentialsquare, 
            //        a.acc_num as accountnumber
            //      FROM regop_pers_acc a, tmp_room r
            //      WHERE a.room_id = r.room_id;

            //      create index ix_tmp_account_1 on tmp_account(gkh_account_id);
            //      analyze tmp_account;

            //      -- обновить данные
            //      update public.ris_account ris set 
            //        (typeaccount, livingpersonsnumber, totalsquare, residentialsquare, heatedarea, 
            //            closed, accountnumber, begindate, is_renter,
            //            object_version, object_edit_date, operation) =  
            //        (t.typeaccount, t.livingpersonsnumber, t.totalsquare, t.residentialsquare, t.heatedarea, 
            //            t.closed, t.accountnumber, t.begindate, t.is_renter,
            //            ris.object_version + 1, now(), 1) 
            //        from tmp_account t 
            //          where ris.external_id = t.gkh_account_id 
            //            and ris.gi_contragent_id = in_ris_contragent_id;

            //      -- вставить данные 
            //      insert into public.ris_account (gi_contragent_id, external_id, 
            //          typeaccount, livingpersonsnumber, totalsquare, residentialsquare, heatedarea, 
            //          closed, accountnumber, begindate, is_renter,
            //          object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //        select in_ris_contragent_id, t.gkh_account_id, 
            //          t.typeaccount, t.livingpersonsnumber, t.totalsquare, t.residentialsquare, t.heatedarea, 
            //          t.closed, t.accountnumber, t.begindate, t.is_renter,
            //          0, now(), now(), 0, 'gkh'
            //        from tmp_account t
            //        where not exists (select 1 from public.ris_account ris where ris.external_id = t.gkh_account_id and ris.gi_contragent_id = in_ris_contragent_id);
  
            //      -- перенос абонентов
            //      PERFORM ris.ris_transfer_account_owner('tmp_account', in_ris_contragent_id);
            //      PERFORM ris.ris_transfer_account_relation('tmp_account', in_ris_contragent_id);
  
            //      PERFORM ris.drop_temp_table('tmp_room');
            //      PERFORM ris.drop_temp_table('tmp_account');
  
            //      error_text := 'ok';  
            //      error_code := 0;
    
            //      return;
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;


            //    CREATE OR REPLACE FUNCTION ris.get_dict_id(in in_action_code text)
            //      RETURNS int AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Получить id справочника из gi_integr_dict по in_action_code
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 06.10.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE 
            //      dict_id integer;
            //    BEGIN
            //      select id into dict_id from gi_integr_dict where trim(upper(action_code)) = trim(upper(in_action_code));
            //      dict_id := COALESCE(dict_id, 0);   
            //      return dict_id;
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;


            //    CREATE OR REPLACE FUNCTION ris.get_gkh_contragent_id(in ris_contragent_id integer)
            //      RETURNS int AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Получить id контрагента из gkh_contragent по ris_contragent_id из gi_contragent
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 06.10.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE 
            //      gkhid integer;
            //    BEGIN
            //      -- код регопа в МЖФ 
            //      select g.gkhid into gkhid  from public.gi_contragent g where g.id = ris_contragent_id;
            //      gkhid := COALESCE(gkhid, 0);   
            //      return gkhid;
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;


            //    CREATE OR REPLACE FUNCTION ris.get_gkh_house_id(in ris_house_id integer)
            //      RETURNS int AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Получить id контрагента из gkh_contragent по ris_contragent_id из gi_contragent
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 06.10.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    DECLARE 
            //      house_id integer;
            //    BEGIN
            //      select h.external_id into house_id from public.ris_house h where h.id = ris_house_id;
            //      house_id := COALESCE(house_id, 0);   
            //      return house_id;
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_acknowledgment(IN in_ris_contragent_id integer, IN in_report_month date, IN in_ris_house_id integer DEFAULT 0, OUT error_code integer, OUT error_text text)
            //      RETURNS record AS
            //    $BODY$
            //    BEGIN
            //      error_text := 'ok';  
            //      error_code := 0;
    
            //      return;
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;


            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_address_info(IN in_ris_contragent_id integer, IN in_report_month date, IN in_ris_house_id integer DEFAULT 0, OUT error_code integer, OUT error_text text)
            //      RETURNS record AS
            //    $BODY$
            //    BEGIN
            //      error_text := 'ok';  
            //      error_code := 0;
    
            //      return;
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;



            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_notiforderexecut(IN in_ris_contragent_id integer, IN in_report_month date, IN in_ris_house_id integer DEFAULT 0, OUT error_code integer, OUT error_text text)
            //      RETURNS record AS
            //    $BODY$
            //    BEGIN
            //      error_text := 'ok';  
            //      error_code := 0;
            //      return;
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;


            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_payment_doc(IN in_ris_contragent_id integer, IN in_report_month date, IN in_ris_house_id integer DEFAULT 0, OUT error_code integer, OUT error_text text)
            //      RETURNS record AS
            //    $BODY$
            //    BEGIN
            //      error_text := 'ok';  
            //      error_code := 0;
    
            //      return;
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;



            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_payment_info(IN in_ris_contragent_id integer, IN in_report_month date, IN in_ris_house_id integer DEFAULT 0, OUT error_code integer, OUT error_text text)
            //      RETURNS record AS
            //    $BODY$
            //    BEGIN
            //      error_text := 'ok';  
            //      error_code := 0;
    
            //      return;
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;


            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_capital_repair_charge(IN in_ris_contragent_id integer, IN in_report_month date, IN in_ris_house_id integer DEFAULT 0, OUT error_code integer, OUT error_text text)
            //      RETURNS record AS
            //    $BODY$
            //    BEGIN
            //      error_text := 'ok';  
            //      error_code := 0;
    
            //      return;
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;



            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_capital_repair_debt(IN in_ris_contragent_id integer, IN in_report_month date, IN in_ris_house_id integer DEFAULT 0, OUT error_code integer, OUT error_text text)
            //      RETURNS record AS
            //    $BODY$
            //    BEGIN
            //      error_text := 'ok';  
            //      error_code := 0;
    
            //      return;
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql;  

            //");
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.ExecuteNonQuery(@"
            //     DROP FUNCTION IF EXISTS ris.drop_temp_table;
            //     DROP FUNCTION IF EXISTS ris.ris_transfer_account_owner;
            //     DROP FUNCTION IF EXISTS ris.ris_transfer_account_relation;
            //     DROP FUNCTION IF EXISTS ris.ris_transfer_account;
            //     DROP FUNCTION IF EXISTS ris.get_dict_id;
            //     DROP FUNCTION IF EXISTS ris.get_gkh_contragent_id;
            //     DROP FUNCTION IF EXISTS ris.get_gkh_house_id;
            //     DROP FUNCTION IF EXISTS ris.ris_transfer_acknowledgment;
            //     DROP FUNCTION IF EXISTS ris.ris_transfer_address_info;
            //     DROP FUNCTION IF EXISTS ris.ris_transfer_notiforderexecut;
            //     DROP FUNCTION IF EXISTS ris.ris_transfer_payment_doc;
            //     DROP FUNCTION IF EXISTS ris.ris_transfer_payment_info;
            //     DROP FUNCTION IF EXISTS ris.ris_transfer_capital_repair_charge;
            //     DROP FUNCTION IF EXISTS ris.ris_transfer_capital_repair_debt;
            //");
        }
    }
}