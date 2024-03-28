namespace Bars.GisIntegration.RegOp.Migrations.Version_2016102700
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016102700")]
    [MigrationDependsOn(typeof(Version_2016102600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.ExecuteNonQuery(@"

            //       CREATE OR REPLACE FUNCTION ris.ris_transfer_account_relation(
            //            in_temp_table_account text,
            //            in_ris_contragent_id integer)
            //          RETURNS text AS
            //        $BODY$
            //        --=================================================================
            //        -- Назначение: Перенос связей лицевых счетов в ris_account_relations
            //        -- Автор: Царегородцева Е.Д.                                               
            //        -- Дата создания: 07.10.2016                                       
            //        -- Дата изменения: 16.10.2016                                    
            //        --=================================================================
            //        DECLARE
            //            sql text;
            //        BEGIN
            //            PERFORM ris.drop_temp_table('tmp_account_relation');

            //            -- выбрать список помещений
            //            execute 'CREATE TEMP TABLE tmp_account_relation as 
            //            SELECT 
            //            t.gkh_account_id,
            //            t.gkh_room_id,
            //            t.gkh_house_id,

            //            ris.id as ris_account_id, 
            //            0 as residential_premise_id, 
            //            0 as ris_house_id
            //            FROM ' || in_temp_table_account || ' t, ris_account ris 
            //            WHERE t.gkh_account_id = ris.external_id 
            //              and ris.gi_contragent_id = ' || in_ris_contragent_id;

            //            create index ix_tmp_account_relation_gkh_ids on tmp_account_relation (gkh_room_id, gkh_house_id);
            //            analyze tmp_account_relation;

            //            UPDATE tmp_account_relation t SET residential_premise_id = ris.id FROM ris_residentialpremises ris WHERE t.gkh_room_id  = ris.external_id; 
            //            UPDATE tmp_account_relation t SET ris_house_id           = ris.id FROM ris_house               ris WHERE t.gkh_house_id = ris.external_id;

            //            CREATE INDEX ix_tmp_account_relation_ris_ids on tmp_account_relation (gkh_account_id, ris_house_id, residential_premise_id);
            //            ANALYZE tmp_account_relation;

            //            -- вставить данные
            //            insert into public.ris_account_relations (gi_contragent_id, 
            //            external_id, 
            //            account_id, house_id, residential_premise_id, nonresidential_premise_id, living_room_id,
            //            object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //            select in_ris_contragent_id, 
            //            t.gkh_account_id, 
            //            t.ris_account_id, t.ris_house_id, t.residential_premise_id, null, null,
            //            0, now(), now(), 0, 'ris'
            //            from tmp_account_relation t
            //            where not exists (select 1 from public.ris_account_relations ris where ris.external_id = t.gkh_account_id and ris.gi_contragent_id = in_ris_contragent_id)
            //            and t.ris_house_id > 0 and t.residential_premise_id > 0;

            //            PERFORM ris.drop_temp_table('tmp_account_relation');

            //            return 'ok';
            //        END;
            //        $BODY$
            //          LANGUAGE plpgsql VOLATILE
            //          COST 100;
            //        ALTER FUNCTION ris.ris_transfer_account_relation(text, integer)
            //          OWNER TO bars;

            //        -- Function: ris.ris_transfer_capital_repair_debt(text, integer, date, integer)

            //        -- DROP FUNCTION ris.ris_transfer_capital_repair_debt(text, integer, date, integer);

            //        CREATE OR REPLACE FUNCTION ris.ris_transfer_capital_repair_debt(
            //            IN tmp_account_table text,
            //            IN in_ris_contragent_id integer,
            //            IN in_report_month date,
            //            IN in_ris_house_id integer DEFAULT 0,
            //            OUT error_code integer,
            //            OUT error_text text)
            //          RETURNS record AS
            //        $BODY$
            //        --=================================================================
            //        -- Назначение: Перенос долгов по кап. ремонту
            //        -- Автор: Царегородцева Е.Д.                                               
            //        -- Дата создания: 08.10.2016                                     
            //        -- Дата изменения:                                      
            //        --=================================================================
            //        DECLARE
            //          p_period_id integer;
            //          sql text;
            //        BEGIN
            //          in_ris_house_id := coalesce(in_ris_house_id, 0);

            //          if in_report_month is null then
            //            error_text := 'Не передан отчетный период';  
            //            error_code := -1;
            //            return;
            //          end if;

            //          p_period_id := ris.get_regop_period_id(in_report_month);

            //          if p_period_id <= 0 then
            //            error_text := 'Не удалось определить отчетный период';  
            //            error_code := -1;
            //            return;
            //          end if;
  
            //          PERFORM ris.drop_temp_table('tmp_charge_debt');

            //          -- получить данные для ЕПД
            //          execute 'CREATE TEMP TABLE tmp_charge_debt as   
            //          SELECT
            //            ch.id as external_id,
            //            t.gkh_account_id,  
            //            ris.get_external_id(t.gkh_account_id, ' || quote_literal(in_report_month) || ') as gkh_payment_doc_id,
            //            0 as ris_payment_doc_id,

            //              -- начислено взносов по минимальному тарифу всего
            //            coalesce(ch.CHARGE_BASE_TARIFF, 0) + coalesce(ch.BALANCE_CHANGE, 0) + coalesce(ch.RECALC, 0) as charged_base_tariff, 
            //              -- начислено взносов по тарифу решения всего 
            //            coalesce(ch.CHARGE_TARIFF, 0) + coalesce(ch.CHARGE_BASE_TARIFF, 0) + coalesce(ch.RECALC_DECISION, 0) as charged_decision_tariff, 
            //              -- начислено пени всего
            //            coalesce(ch.PENALTY, 0) + coalesce(ch.RECALC_PENALTY, 0) as charged_penalty,
            //              -- задолженность по взносам всего
            //            coalesce(ch.CHARGE_BASE_TARIFF, 0) + coalesce(ch.BALANCE_CHANGE, 0) + coalesce(ch.recalc, 0) - coalesce(ch.TARIFF_PAYMENT, 0) as debt_base_tariff, 
            //              -- задолженность по взносам тарифа решения всего
            //            coalesce(ch.CHARGE_TARIFF, 0) - coalesce(ch.CHARGE_BASE_TARIFF, 0) + coalesce(ch.RECALC_DECISION, 0) - coalesce(ch.TARIFF_DESICION_PAYMENT, 0) as debt_decision_tariff, 
            //              -- Задолженность пени всего
            //            coalesce(ch.PENALTY, 0) + coalesce(ch.RECALC_PENALTY, 0) - coalesce(ch.penalty_payment, 0) as debt_penalty, 

            //            extract(month from ' || quote_literal(in_report_month) || '::DATE)::integer as month_,
            //            extract(year from ' || quote_literal(in_report_month) || '::DATE)::integer as year_,
    
            //            0.00 as total_payable
            //          FROM ' || tmp_account_table || ' t, 
            //            REGOP_PERS_ACC_PERIOD_SUMM ch
            //          where t.gkh_account_id = ch.account_id
            //            and ch.period_id = ' || p_period_id;

            //          create index ix_tmp_charge_debt_1 on tmp_charge_debt (gkh_payment_doc_id, gkh_account_id, external_id);
            //          analyze tmp_charge_debt;

            //          UPDATE tmp_charge_debt t set ris_payment_doc_id = ris.id FROM ris_payment_doc ris WHERE ris.external_id = t.gkh_payment_doc_id;  
  
            //          -- ""Начислено взносов по минимальному тарифу всего"" + ""Начислено взносов по тарифу решения всего"" + ""Начислено пени всего"" + ""Задолженность по взносам всего"" + Задолженность по взносам тарифа решения всего"" + ""Задолженность пени всего""
            //          UPDATE tmp_charge_debt t set
            //            total_payable = t.charged_base_tariff + t.charged_decision_tariff + t.charged_penalty + t.debt_base_tariff + t.debt_decision_tariff + t.debt_penalty;

            //                    --обновить данные
            //                   UPDATE public.ris_capital_repair_debt ris set
            //                     (year, month, total_payable, object_version, object_edit_date, operation) =  
            //            (tt.year_, tt.month_, tt.total_payable, ris.object_version + 1, now(), 1 ) 
            //            from tmp_charge_debt tt
            //              where ris.external_id = tt.external_id
            //                and ris.gi_contragent_id = in_ris_contragent_id
            //                and tt.ris_payment_doc_id > 0
            //                and ris.external_system_name = 'gkh';

            //          -- вставить данные
            //          insert into public.ris_capital_repair_debt(
            //             gi_contragent_id,
            //             external_id,
            //             payment_doc_id,
            //             month,
            //             year,
            //             total_payable,
            //             object_version, object_create_date, object_edit_date, operation, external_system_name)
            //          select
            //            in_ris_contragent_id,
            //            tt.external_id, 
            //            tt.ris_payment_doc_id, 
            //            tt.month_,
            //            tt.year_,
            //            tt.total_payable,
            //            0, now(), now(), 0, 'gkh'
            //          from tmp_charge_debt tt
            //          where not exists(select 1 from public.ris_capital_repair_debt ris where ris.external_id = tt.external_id and ris.gi_contragent_id = in_ris_contragent_id)
            //            and tt.ris_payment_doc_id > 0;

            //                PERFORM ris.drop_temp_table('tmp_charge_debt');

            //                error_text := 'ok';  
            //          error_code := 0;
    
            //          return;
            //        END;
            //        $BODY$
            //          LANGUAGE plpgsql VOLATILE
            //          COST 100;
            //                ALTER FUNCTION ris.ris_transfer_capital_repair_debt(text, integer, date, integer)
            //          OWNER TO bars;
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