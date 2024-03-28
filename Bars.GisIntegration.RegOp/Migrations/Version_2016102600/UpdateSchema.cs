namespace Bars.GisIntegration.RegOp.Migrations.Version_2016102600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016102600")]
    [MigrationDependsOn(typeof(Version_2016102401.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.ExecuteNonQuery(@"

            //    CREATE OR REPLACE FUNCTION ris.ris_transfer_payment_info(
            //        IN tmp_account_table text,
            //        IN in_ris_contragent_id integer,
            //        IN in_report_month date,
            //        IN in_ris_house_id integer DEFAULT 0,
            //        OUT error_code integer,
            //        OUT error_text text)
            //      RETURNS record AS
            //    $BODY$
            //                    --=================================================================
            //                    -- Назначение: Перенос данных расчетных счетов, по которым выставлены ЕПД
            //                    -- Автор: Царегородцева Е.Д.                                               
            //                    -- Дата создания: 10.10.2016                                  
            //                    -- Дата изменения: 26.10.2016                             
            //                    --=================================================================
            //                    DECLARE
            //                        p_report_id integer;
            //                        sql text;
            //                        p_gkh_contragent_id integer;
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

            //                        p_gkh_contragent_id := ris.get_gkh_contragent_id(in_ris_contragent_id);
  
            //                        PERFORM ris.drop_temp_table('tmp_bank_account');
            //                        PERFORM ris.drop_temp_table('tmp_payment_info');
            //                        PERFORM ris.drop_temp_table('tmp_bank_oper_account');

            //                        -- выбрать р/с
            //                        execute 'CREATE TEMP TABLE tmp_bank_oper_account AS
            //                        SELECT rs.id, rs.account_owner_id, rs.credit_org_id, rs.account_number
            //                        FROM ' || tmp_account_table || ' t, REGOP_CALC_ACC_RO ar, REGOP_CALC_ACC rs
            //                        WHERE t.gkh_house_id = ar.ro_id
            //                        and ar.account_id = rs.id
            //                        and rs.account_owner_id = ' || p_gkh_contragent_id  || '
            //                        GROUP BY rs.id, rs.account_owner_id, rs.credit_org_id, rs.account_number';

            //                        -- данные для РИС
            //                        CREATE TEMP TABLE tmp_payment_info AS
            //                        SELECT 
            //                        rs.id as gkh_bank_account_id, 
            //                        bank.bik as bank_bik,
            //                        bank.corr_account as correspondent_bank_account,
            //                        bank.name as bank_name,
            //                        COALESCE(rs.account_number, '') as operating_account_number,
            //                        c.inn as recipient_inn,
            //                        c.kpp as recipient_kpp,
            //                        c.name as recipient
            //                        from tmp_bank_oper_account rs, gkh_contragent c, ovrhl_credit_org bank
            //                        where rs.account_owner_id = c.id
            //                        and rs.credit_org_id = bank.id;

            //                        create index ix_tmp_payment_info_1 on tmp_payment_info (gkh_bank_account_id);
            //                        analyze tmp_payment_info;

            //                        -- обновить данные
            //                        UPDATE public.ris_payment_info ris SET 
            //                        (recipient, recipient_kpp, recipient_inn,
            //                            bank_bik, bank_name, operating_account_number, correspondent_bank_account,
            //                            object_version, object_edit_date, operation) =  
            //                        (tt.recipient, tt.recipient_kpp, tt.recipient_inn,
            //                            tt.bank_bik, tt.bank_name, tt.operating_account_number, tt.correspondent_bank_account,
            //                            ris.object_version + 1, now(), 1 ) 
            //                        FROM tmp_payment_info tt
            //                        WHERE 
            //                                ris.external_id = tt.gkh_bank_account_id 
            //                            and ris.gi_contragent_id = in_ris_contragent_id
            //                            and ris.external_system_name = 'gkh';

            //                        -- вставить данные 
            //                        INSERT INTO public.ris_payment_info (gi_contragent_id, external_id, 
            //                            recipient, recipient_kpp, recipient_inn,
            //                            bank_bik, bank_name, operating_account_number, correspondent_bank_account,
            //                            object_version, object_create_date, object_edit_date, operation, external_system_name) 
            //                        SELECT in_ris_contragent_id, tt.gkh_bank_account_id, 
            //                        tt.recipient, tt.recipient_kpp, tt.recipient_inn,
            //                        tt.bank_bik, tt.bank_name, tt.operating_account_number, tt.correspondent_bank_account,
            //                        0, now(), now(), 0, 'gkh'
            //                        FROM tmp_payment_info tt
            //                        WHERE 
            //                        NOT EXISTS (SELECT 1 FROM public.ris_payment_info ris WHERE 
            //                            ris.external_id = tt.gkh_bank_account_id and ris.gi_contragent_id = in_ris_contragent_id);

            //                        PERFORM ris.drop_temp_table('tmp_bank_account');
            //                        PERFORM ris.drop_temp_table('tmp_payment_info');
            //                        PERFORM ris.drop_temp_table('tmp_bank_oper_account');
  
            //                        error_text := 'ok';  
            //                        error_code := 0;
    
            //                        return;
            //                    END;
            //                    $BODY$
            //      LANGUAGE plpgsql VOLATILE
            //      COST 100;
            //    ALTER FUNCTION ris.ris_transfer_payment_info(text, integer, date, integer)
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