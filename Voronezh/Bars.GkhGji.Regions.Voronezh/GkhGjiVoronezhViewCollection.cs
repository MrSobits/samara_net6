namespace Bars.GkhGji.Regions.Voronezh
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;
    using Bars.GkhGji.Regions.BaseChelyabinsk;

    public class GkhGjiVoronezhViewCollection : GkhGjiChelyabinskViewCollection
    {
        public override int Number => 1;

        #region Create

        #region Views

        /// <summary>
        /// Вьюха обращений граждан
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewAppealCits(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.PostgreSql)
            {
                return @"
                    DROP VIEW IF EXISTS view_gji_appeal_cits;
                    CREATE VIEW view_gji_appeal_cits AS 
                        SELECT 
                            gac.id, 
                            gac.DOCUMENT_NUMBER, 
                            gac.gji_number,
                            gac.GJI_NUM_SORT, 
                            gac.date_from, 
                            gac.check_time, 
                            gac.questions_count, 
                            countro.count_ro, 
                            mun.municipality, 
                            ''::varchar(300) as mo_settlement,
                            ''::varchar(50) as place_name,
                            c.name AS 
                            contragent_name, 
                            gac.state_id,
                            gac.executant_id, 
                            gac.tester_id, 
                            gac.SURETY_RESOLVE_ID,
                            gac.EXECUTE_DATE,
                            gac.SURETY_DATE,
                            gjiGetAppealRobject(gac.id) AS ro_ids,
                            gac.ZONAINSP_ID,
                            gjiGetRobjectAdrAppeal(gac.id) AS ro_adr,
                            gac.correspondent,
                            gac.correspondent_address,
                            mun.municipality_id,
                            gac.EXTENS_TIME,
	                        gjiGetRevenueSourceNames(gac.id) as source_names,
                            gjiGetRevenueSourceDates(gac.id::bigint) AS source_dates,
                            gjiGetRevenueSourceNumbers(gac.id::bigint) AS source_numbers,
                            gjigetappealexecutants(gac.id) as executant_names,
                            gjigetappealsubjects(gac.id) as subjects,
                            gjiGetSubSubjectsName(gac.id::bigint) AS SubSubjects_name,
                            gjiGetFeaturesName(gac.id::bigint) AS Features_name,
                            gjiGetControllerFio(gac.id::bigint) AS controller_fio
                        FROM gji_appeal_citizens gac
                            LEFT JOIN ( 
                                SELECT 
                                    count(garo.reality_object_id) AS count_ro, 
                                    gac1.id AS gac_id
                                FROM gji_appeal_citizens gac1
                                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                                    GROUP BY gac1.id
                                ) countro ON countro.gac_id = gac.id
                            LEFT JOIN ( 
                                SELECT 
                                    gaac.id AS gac_id, 
                                    ( 
                                        SELECT 
                                            gdm.name AS municipality
                                        FROM gji_appeal_citizens gac1
                                            JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                                            JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                                            JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                                            WHERE gac1.id = gaac.id
                                        LIMIT 1
                                    ) AS municipality,
                                    ( 
                                        SELECT 
                                            gdm.id AS municipality_id
                                        FROM gji_appeal_citizens gac1
                                            JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                                            JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                                            JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                                            WHERE gac1.id = gaac.id
                                        LIMIT 1
                                    ) AS municipality_id
                                FROM gji_appeal_citizens gaac
                                ) mun ON mun.gac_id = gac.id
                            LEFT JOIN gkh_managing_organization mo ON mo.id = gac.managing_org_id
                            LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id";
            }

            return null;
        }
        #endregion Views

        #region Functions
        private string CreateFunctionGjiGetAppealExecutants(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.PostgreSql)
            {
                return @"
                        CREATE OR REPLACE FUNCTION gjigetappealexecutants(bigint)
                      RETURNS text AS
                    $BODY$ 
                    declare 
                       objectId text :='';
                       result text := '';
                       cursorCon CURSOR IS 
                       select ins.fio 
                       from gji_appcit_executant ex
                            left join gkh_dict_inspector ins
                                on ins.id = ex.executant_id
                       where ex.executant_id is not null
                            and ex.appcit_id = $1; 
                    begin 
                       OPEN cursorCon;
                       loop
                       FETCH cursorCon INTO objectId;
                       EXIT WHEN not FOUND; 
                       result:=result||case when result <> '' then ', ' else '' end||objectId; 
                       end loop;
                       CLOSE cursorCon; 
                       return result; 
                    end; 
                    $BODY$
                      LANGUAGE plpgsql VOLATILE
                      COST 100;";
            }
            return null;
        }

        private string CreateFunctionGjiGetAppealSubjects(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.PostgreSql)
            {
                return @"
                       CREATE OR REPLACE FUNCTION gjigetappealsubjects(bigint)
                      RETURNS text AS
                    $BODY$ 
                    declare 
                       objectId text :='';
                       result text := '';
                       cursorCon CURSOR IS 
                       select st_subj.name
                       from gji_appcit_statsubj subj
                            left join gji_dict_statement_subj st_subj
                                on st_subj.id = subj.statement_subject_id
                       where subj.appcit_id = $1; 
                    begin 
                       OPEN cursorCon;
                       loop
                       FETCH cursorCon INTO objectId;
                       EXIT WHEN not FOUND; 
                       result:=result||case when result <> '' then ', ' else '' end||objectId; 
                       end loop;
                       CLOSE cursorCon; 
                       return result; 
                    end; 
                    $BODY$
                      LANGUAGE plpgsql VOLATILE
                      COST 100";
            }
            return null;
        }
        #endregion Functions
        #endregion Create

        #region Delete
        #region Views
        private string DeleteViewAppealCits(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.PostgreSql)
            {
                return this.DropViewPostgreQuery("view_gji_appeal_cits");
            }

            return null;
        }

        #endregion Views

        #region Functions
        /// <summary>
        /// Удаление функции gjigetappealexecutants(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGjiGetAppealExecutants(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return null;
            }

            return @"DROP FUNCTION if exists gjigetappealexecutants(bigint)";
        }

        /// <summary>
        /// Удаление функции gjigetappealsubjects(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGjiGetAppealSubjects(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return null;
            }

            return @"DROP FUNCTION if exists gjigetappealsubjects(bigint)";
        }
        #endregion Functions
        #endregion Delete
    }
}
