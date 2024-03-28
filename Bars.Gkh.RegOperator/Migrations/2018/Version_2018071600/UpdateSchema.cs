namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018071600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2018071600")]
   
    [MigrationDependsOn(typeof(Version_2018071000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            UpdateView();
        }
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public void UpdateView()
        {
            Database.ExecuteNonQuery(@"
            -- Function: public.getviewindividualclaimwork()

            -- DROP FUNCTION public.getviewindividualclaimwork();
            BEGIN TRANSACTION
            ISOLATION LEVEL READ COMMITTED
            READ WRITE
            DEFERRABLE;

            DROP VIEW IF EXISTS view_clw_individual_claim_work;

            DROP FUNCTION IF EXISTS public.getviewindividualclaimwork();

            CREATE OR REPLACE FUNCTION public.getviewindividualclaimwork()
                RETURNS TABLE
                        (id bigint,
                        debtor_state integer,
                        state_id bigint,
                        municipality character varying,
                        registration_address character varying,
                        account_owner_name character varying,
                        accounts_number integer,
                        accounts_address character varying,
                        cur_charge_base_tariff_debt numeric,
                        cur_charge_decision_tariff_debt numeric,
                        cur_charge_debt numeric,
                        cur_penalty_debt numeric,
                        is_debt_paid boolean,
                        debt_paid_date date,
                        pir_create_date date,
                        first_document_date date,
                        has_charges_185fz boolean,
                        min_share numeric,
                        user_id bigint) AS

                        $BODY$ DECLARE
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
                        gro.has_charges_185fz,
                        min(rpa_all.area_share) min_share,
                        string_agg (( gro.address || ', кв. ' || rm.croom_num ) :: TEXT, ', ' ) ad 
                        FROM CLW_CLAIM_WORK_ACC_DETAIL cwd
                            JOIN REGOP_PERS_ACC ac ON ac.ID = cwd.account_id
                            JOIN GKH_ROOM rm ON rm.ID = ac.room_id
                            JOIN GKH_REALITY_OBJECT gro ON gro.ID = rm.ro_id 

                            join regop_pers_acc_owner rpao on rpao.id=ac.acc_owner_id
                            join regop_pers_acc rpa_all on rpao.id=rpa_all.acc_owner_id
                        GROUP BY 1,2;
                        CREATE INDEX ind_works ON works ( claim_work_id );
                        ANALYZE works;


                        DROP TABLE IF EXISTS clw_dates;
                        CREATE TEMP TABLE clw_dates as SELECT
                        ccw.id id,
                        cdcw.pir_create_date pir_create_date,
                        min(cd.document_date) first_document_date
                        from
                        clw_debtor_claim_work cdcw
                        join clw_claim_work ccw on ccw.id=cdcw.id
                        left join clw_document cd on ccw.id=cd.claimwork_id
                        group by 1,2;
                        CREATE INDEX ind_clw_dates on clw_dates(id);
                        ANALYZE clw_dates;

                        DROP TABLE IF EXISTS svod;
                        CREATE TEMP TABLE svod AS SELECT
                        cw.ID AS ID,
                        cbw.debtor_state as debtor_state,
                        cw.state_id AS state_id,
                        COALESCE (mu.NAME,
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
                        works.has_charges_185fz,
                        works.min_share,
                        cd.pir_create_date,
                        cd.first_document_date,
                        cbw.user_id AS user_id 

                        FROM CLW_CLAIM_WORK cw
                            JOIN CLW_DEBTOR_CLAIM_WORK cbw ON cw.ID = cbw.ID
                            JOIN ( CLW_INDIVIDUAL_CLAIM_WORK icw JOIN works ON works.claim_work_id = icw.ID ) ON icw.ID = cbw.ID
                            JOIN REGOP_PERS_ACC_OWNER own ON own.ID = cbw.owner_id
                            JOIN REGOP_INDIVIDUAL_ACC_OWN iown ON iown.ID = own.ID
                            LEFT JOIN GKH_REALITY_OBJECT ro ON ro.ID = iown.registration_ro_id
                            LEFT JOIN GKH_DICT_MUNICIPALITY mu ON mu.ID = ro.municipality_id
                            LEFT JOIN CLW_DATES cd on cd.id=cw.id;

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
                        pir_create_date = rec.pir_create_date;
                        first_document_date = rec.first_document_date;
                        has_charges_185fz = rec.has_charges_185fz;
                        min_share = rec.min_share;
                        user_id = rec.user_id;
                        RETURN NEXT;

                        END loop;
                        END;
                        $BODY$
            LANGUAGE plpgsql VOLATILE
            COST 100
            ROWS 1000;
            ALTER FUNCTION public.getviewindividualclaimwork()
            OWNER TO postgres;


            CREATE OR REPLACE VIEW public.view_clw_individual_claim_work AS 
            SELECT getviewindividualclaimwork.id,
                getviewindividualclaimwork.debtor_state,
                getviewindividualclaimwork.state_id,
                getviewindividualclaimwork.municipality,
                getviewindividualclaimwork.registration_address,
                getviewindividualclaimwork.account_owner_name,
                getviewindividualclaimwork.accounts_number,
                getviewindividualclaimwork.accounts_address,
                getviewindividualclaimwork.cur_charge_base_tariff_debt,
                getviewindividualclaimwork.cur_charge_decision_tariff_debt,
                getviewindividualclaimwork.cur_charge_debt,
                getviewindividualclaimwork.cur_penalty_debt,
                getviewindividualclaimwork.is_debt_paid,
                getviewindividualclaimwork.debt_paid_date,
                getviewindividualclaimwork.pir_create_date,
                getviewindividualclaimwork.first_document_date,
                getviewindividualclaimwork.has_charges_185fz,
                getviewindividualclaimwork.min_share,
                getviewindividualclaimwork.user_id
                FROM getviewindividualclaimwork() 
                getviewindividualclaimwork
                (id,
                debtor_state,
                state_id,
                municipality,
                registration_address,
                account_owner_name,
                accounts_number,
                accounts_address,
                cur_charge_base_tariff_debt,
                cur_charge_decision_tariff_debt,
                cur_charge_debt,
                cur_penalty_debt,
                is_debt_paid,
                debt_paid_date,
                pir_create_date,
                first_document_date,
                has_charges_185fz,
                min_share,
                user_id);

            ALTER TABLE public.view_clw_individual_claim_work
            OWNER TO postgres;

            commit;            
            ");
        }
    }
}