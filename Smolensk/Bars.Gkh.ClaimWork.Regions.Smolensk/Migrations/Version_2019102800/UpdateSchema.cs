namespace Bars.Gkh.ClaimWork.Regions.Smolensk.Migrations.Version_2019102800
{
    using System;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [Migration("2019122100")]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
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
            -- FUNCTION: public.getviewindividualclaimwork()

-- DROP FUNCTION public.getviewindividualclaimwork();

CREATE OR REPLACE FUNCTION public.getviewindividualclaimwork(
	)
RETURNS TABLE(id bigint, debtor_state integer, state_id bigint, municipality character varying, registration_address character varying, account_owner_name character varying, accounts_number integer, accounts_address character varying, cur_charge_base_tariff_debt numeric, cur_charge_decision_tariff_debt numeric, cur_charge_debt numeric, cur_penalty_debt numeric, is_debt_paid boolean, debt_paid_date date, pir_create_date date, first_document_date date, has_charges_185fz boolean, min_share numeric, underage boolean, user_id bigint) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE 
    ROWS 1000
AS $BODY$ DECLARE
                  rec record;
                BEGIN

                  DROP TABLE IF EXISTS mun;
                  CREATE TEMP TABLE mun AS
                    SELECT i.ID, M.NAME
                    FROM CLW_INDIVIDUAL_CLAIM_WORK i
                           JOIN CLW_CLAIM_WORK_ACC_DETAIL cd ON i.ID = cd.claim_work_id
                           JOIN regop_pers_acc pa ON pa.ID = cd.account_id
                           JOIN gkh_room gr ON gr.ID = pa.room_id
                           JOIN gkh_reality_object r ON r.ID = gr.ro_id
                           JOIN gkh_dict_municipality M ON M.ID = r.municipality_id
                    GROUP BY i.ID,
                             M.NAME;
                  CREATE INDEX ind_mun
                    ON mun (ID);
                  ANALYZE mun;

                  DROP TABLE IF EXISTS works;
                  CREATE TEMP TABLE works AS
                SELECT cwd.claim_work_id,
                           false has_charges_185fz,
                           --min(rpa_all.area_share)                                             min_share,
                           1                                             min_share,


                           --bool_or(case
                            --         when birth_date = '' then false
                            --         when (now() - birth_date :: timestamp) < ('18 years' :: interval) then true
                            --         else false end) as                                        underage,
			false	underage,
                           (gro.address || ', кв. ' || rm.croom_num) ad
                    FROM CLW_CLAIM_WORK_ACC_DETAIL cwd
                           JOIN REGOP_PERS_ACC ac ON ac.ID = cwd.account_id
                           JOIN GKH_ROOM rm ON rm.ID = ac.room_id
                           JOIN GKH_REALITY_OBJECT gro ON gro.ID = rm.ro_id
                           --left join regop_lawsuit_owner_info rloi on rloi.personal_account_id = ac.id
                           --left join regop_lawsuit_ind_owner_info rlioi on rlioi.id = rloi.id
                           --join regop_pers_acc_owner rpao on rpao.id = ac.acc_owner_id
                           --join regop_pers_acc rpa_all on rpao.id = rpa_all.acc_owner_id
                    GROUP BY 1, 2,5;
                  CREATE INDEX ind_works
                    ON works (claim_work_id);
                  ANALYZE works;

                  DROP TABLE IF EXISTS clw_dates;
                  CREATE TEMP TABLE clw_dates as
                    SELECT ccw.id id, cdcw.pir_create_date pir_create_date, min(cd.document_date) first_document_date
                    from clw_debtor_claim_work cdcw
                           join clw_claim_work ccw on ccw.id = cdcw.id
                           left join clw_document cd on ccw.id = cd.claimwork_id
                    group by 1, 2;
                  CREATE INDEX ind_clw_dates
                    on clw_dates (id);
                  ANALYZE clw_dates;

                  DROP TABLE IF EXISTS svod;
                  CREATE TEMP TABLE svod AS
                    SELECT cw.ID                                                                                   AS ID,
                           cbw.debtor_state                                                                        as debtor_state,
                           cw.state_id                                                                             AS state_id,
                           COALESCE(mu.NAME,
                                    (
                                        CASE
                                          WHEN (SELECT COUNT(*)
                                                FROM mun
                                                WHERE mun.ID = icw.ID) = 1 THEN (SELECT mun.NAME
                                                                                 FROM mun
                                                                                 WHERE mun.ID = icw.ID)
                                          ELSE''
                                            END
                                        ))                                                                         AS municipality,
                           COALESCE(ro.address, '')                                                                AS registration_address,
                           own.NAME                                                                                AS account_owner_name,
                           (SELECT COUNT(*) FROM CLW_CLAIM_WORK_ACC_DETAIL cwd WHERE cwd.claim_work_id = icw.ID)   AS accounts_number,
                           works.ad                                                                                AS accounts_address,
                           cbw.CUR_CHARGE_BASE_TARIFF_DEBT,
                           cbw.CUR_CHARGE_DECISION_TARIFF_DEBT,
                           cbw.CUR_CHARGE_DEBT,
                           cbw.CUR_PENALTY_DEBT,
                           cw.IS_DEBT_PAID,
                           cw.DEBT_PAID_DATE,
                           works.has_charges_185fz,
                           works.min_share,
                           works.underage,
                           cd.pir_create_date,
                           cd.first_document_date,
                           cbw.user_id                                                                             AS user_id

                    FROM CLW_CLAIM_WORK cw
                           JOIN CLW_DEBTOR_CLAIM_WORK cbw ON cw.ID = cbw.ID
                           JOIN (CLW_INDIVIDUAL_CLAIM_WORK icw JOIN works ON works.claim_work_id = icw.ID) ON icw.ID = cbw.ID
                           JOIN REGOP_PERS_ACC_OWNER own ON own.ID = cbw.owner_id
                           JOIN REGOP_INDIVIDUAL_ACC_OWN iown ON iown.ID = own.ID
                           LEFT JOIN GKH_REALITY_OBJECT ro ON ro.ID = iown.registration_ro_id
                           LEFT JOIN GKH_DICT_MUNICIPALITY mu ON mu.ID = ro.municipality_id
                           LEFT JOIN CLW_DATES cd on cd.id = cw.id;

                  FOR rec IN EXECUTE 'select * from svod'
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
                    underage = rec.underage;
                    user_id = rec.user_id;
                    RETURN NEXT;

                  END loop;
                END;
                $BODY$;

ALTER FUNCTION public.getviewindividualclaimwork()
    OWNER TO postgres;            
            ");
        }
    }
}
    