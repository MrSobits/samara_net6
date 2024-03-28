namespace Bars.GisIntegration.RegOp.Migrations.Version_2016102400
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016102400")]
    [MigrationDependsOn(typeof(Version_2016102000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.ExecuteNonQuery(@"
            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_address_info(
            //        IN tmp_account_table text,
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //      RETURNS record AS
            //    $BODY$
            //                    --=================================================================
            //                    -- Назначение: Перенос площадей и количества проживающих из ЕПД
            //                    -- Автор: Царегородцева Е.Д.                                               
            //                    -- Дата создания: 27.05.2016                                     
            //                    -- Дата изменения: 24.10.2016                                     
            //                    --=================================================================
            //                    DECLARE
            //                        p_report_id integer;
            //                        sql text;
            //                    BEGIN
            //                        in_ris_house_id := coalesce(in_ris_house_id, 0);

            //                        if in_report_month is null then
            //                        error_text := 'Не передан отчетный период';  
            //                        error_code := -1;
            //                        return;
            //                        end if;

            //                        p_report_id := ris.get_regop_period_id(in_report_month);

            //                        if p_report_id <= 0 then
            //                        error_text := 'Не удалось определить отчетный период';  
            //                        error_code := -1;
            //                        return;
            //                        end if;
  
            //                        PERFORM ris.drop_temp_table('tmp_pay_doc');

            //                        -- получить данные для ЕПД
            //                        execute 'CREATE TEMP TABLE tmp_pay_doc as   
            //                        select 
            //                            ris.get_external_id(t.gkh_account_id, ' || quote_literal(in_report_month) || ') as pay_doc_id,
            //                            0 as livingpersonsnumber,
            //                            t.larea as residentialsquare,
            //                            0 as heatedarea,
            //                            t.carea as totalsquare
            //                        FROM ' || tmp_account_table || ' t
            //                        WHERE exists (SELECT 1 FROM REGOP_PERS_ACC_PERIOD_SUMM s where s.account_id = t.gkh_account_id AND s.period_id = ' || p_report_id || ')';

            //                        create index ix_tmp_pay_doc_pay_doc_id on tmp_pay_doc (pay_doc_id);
            //                        analyze tmp_pay_doc;

            //                        -- обновить данные
            //                        update public.ris_address_info ris set 
            //                        (living_person_number, residential_square, heated_area, total_square, object_version, object_edit_date, operation) =  
            //                        (tt.livingpersonsnumber, tt.residentialsquare, tt.heatedarea, tt.totalsquare, ris.object_version + 1, now(), 1 ) 
            //                        from tmp_pay_doc tt
            //                            where ris.external_id = tt.pay_doc_id 
            //                            and ris.gi_contragent_id = in_ris_contragent_id
            //                            and ris.external_system_name = 'gkh';

            //                        -- вставить данные 
            //                        insert into public.ris_address_info (gi_contragent_id, external_id, 
            //                            living_person_number, residential_square, heated_area, total_square,
            //                            object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //                        select in_ris_contragent_id, tt.pay_doc_id, 
            //                        tt.livingpersonsnumber, tt.residentialsquare, tt.heatedarea, tt.totalsquare,
            //                        0, now(), now(), 0, 'gkh'
            //                        from tmp_pay_doc tt
            //                        where not exists (select 1 from public.ris_address_info ris where ris.external_id = tt.pay_doc_id and ris.gi_contragent_id = in_ris_contragent_id);

            //                        PERFORM ris.drop_temp_table('tmp_pay_doc');
  
            //                        error_text := 'ok';  
            //                        error_code := 0;
    
            //                        return;
            //                    END;
            //                    $BODY$
            //      LANGUAGE plpgsql VOLATILE
            //      COST 100;
            //    ALTER FUNCTION ris.ris_transfer_address_info(text, integer, date, integer)
            //      OWNER TO bars;

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